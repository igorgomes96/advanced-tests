#!/bin/bash
docker network create --driver bridge ecommerce || true
docker compose -f docker-compose.yml up -d 

