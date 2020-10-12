#docker login pkvcr.azurecr.io
#kubectl create secret docker-registry pkvcrsec --docker-server=<container-registry-name>.azurecr.io --docker-username=<service-principal-ID> --docker-password=<service-principal-password>
docker tag fastapi:dev pkvcr.azurecr.io/blkhost/fastapi:latest
docker push pkvcr.azurecr.io/blkhost/fastapi


#!/bin/bash
IMAGE_NAME=pkvcr.azurecr.io/blkhost/fastapi

echo $IMAGE_NAME
docker build -t $IMAGE_NAME -f ./Dockerfile  .
az acr login -n xchangeregistry
docker push $IMAGE_NAME
FULL_IMAGE_NAME=$(docker inspect --format='{{index .RepoDigests 0}}' $IMAGE_NAME)
echo $FULL_IMAGE_NAME