DNS="1persondev"
DNS_ZONE="$DNS.com"
DNS_IP_NAME="$DNS-ip"
K8_CLUSTER_NAME=pkvcluster
CLUSTER_MC_RG=main
APP_IDENTIFIER=blkhost
APP_NAMESPCAE="${APP_IDENTIFIER}-ns"

az network public-ip create --resource-group $CLUSTER_MC_RG --name $DNS_IP_NAME --allocation-method static --output none

PublicIP="$(az network public-ip show -g "$CLUSTER_MC_RG" -n "$DNS_IP_NAME" --query "{address: ipAddress}" -o tsv)"  
echo "address:  ${PublicIP}"

az network dns record-set a add-record -g $CLUSTER_MC_RG -z $DNS_ZONE -n @ -a $PublicIP  --output none
echo "dns record set for:  ${DNS_ZONE}"

az network dns record-set a add-record -g $CLUSTER_MC_RG -z $DNS_ZONE -n www -a $PublicIP  --output none
echo "dns record set for: www --> ${DNS_ZONE}"

az network dns record-set a add-record -g $CLUSTER_MC_RG -z $DNS_ZONE -n $APP_IDENTIFIER  -a $PublicIP  --output none
echo "dns record set for: $APP_IDENTIFIER --> ${DNS_ZONE}"

kubectl create namespace blkhost-ns

kubectl config set-context pkvcluster --namespace=blkhost-ns 

helm repo add nginx-stable https://helm.nginx.com/stable
helm repo update

helm install blkhost-ingctr-release nginx-stable/nginx-ingress --set controller.replicaCount=1 --set rbac.create=true 
#--set controller.service.loadBalancerIP="104.42.21.28" 

helm uninstall blkhost-ingctr-release

helm delete --purge blkhost-ingctr-release
