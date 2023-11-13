1. Auth
docker build -t truongkienan/pocketbook-auth .
docker push truongkienan/pocketbook-auth

kubectl apply -f auth-depl.yml
kubectl rollout restart deployment pocketbook-auth-depl

2. Category service
docker build -t truongkienan/pocketbook-categoryservice .
docker push truongkienan/pocketbook-categoryservice

kubectl apply -f category-depl.yml
kubectl rollout restart deployment pocketbook-category-depl

3. Article service
docker build -t truongkienan/pocketbook-articleservice .
docker push truongkienan/pocketbook-articleservice

kubectl apply -f article-depl.yml
kubectl rollout restart deployment pocketbook-article-depl

6. Ingress
kubectl apply -f ingress.yml 

7. Docker compose
docker-compose build      
 
# Account
1. admin/admin123
2. user01/user01

## MSSQL
# CATEGORY SERVICE:
kubectl create secret generic pocketbook-mssql-category --from-literal=SA_PASSWORD="Pkb@1234561"
Port MSSQL local: 1435

# ARTICLE SERVICE:
kubectl create secret generic pocketbook-mssql-article --from-literal=SA_PASSWORD="Pkb@1234561"
Port MSSQL local: 1436

## RabbitMQ
kubectl apply -f rabbitmq-depl.yml
# local mgmt-port:
15674
# local msg-port:
5674

## Redis
kubectl apply -f redis-depl.yml
# mgmt-port:
6380