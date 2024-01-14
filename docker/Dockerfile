FROM postgres:latest

ENV POSTGRES_DB Auction
ENV POSTGRES_USER postgres
ENV POSTGRES_PASSWORD 1234

COPY script.sql /docker-entrypoint-initdb.d/
