docker run --rm --name identity-db -e POSTGRES_PASSWORD=docker -d -p 5432:5432 -v /C/data/invoicing/identity-db:/var/lib/postgresql/data  postgres
docker run --rm --name customer-db -e MONGO_INITDB_ROOT_USERNAME=root -e MONGO_INITDB_ROOT_PASSWORD=pass -v /C/data/invoicing/customer-db:/data/db -p 27017:27017 -d mongo
