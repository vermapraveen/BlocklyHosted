IMAGE_NAME=pkvcr.azurecr.io/blkhost/fastapi

echo $IMAGE_NAME

docker rm $(docker stop $(docker ps -a -q --filter ancestor="$IMAGE_NAME" --format="{{.ID}}"))
docker image rm $IMAGE_NAME

docker build -t $IMAGE_NAME -f ./fastapi.Dockerfile  .
# docker run -p 8001:80 $IMAGE_NAME

# az acr login -n $CR
docker push $IMAGE_NAME
# FULL_IMAGE_NAME=$(docker inspect --format='{{index .RepoDigests 0}}' $IMAGE_NAME)
# echo $FULL_IMAGE_NAME