# 🐳 .NET 8 API + PostgreSQL with Docker: Command Practice Structure

> 🇧🇷 Procurando a versão em português? [Clique aqui](README.pt.md)

This repository provides a test foundation for developers who want to practice essential Docker commands applied to an ASP.NET Core application with PostgreSQL.

The application was generated using the default Visual Studio template for .NET 8 APIs, which already includes a ready-to-use Dockerfile. Simple GET and POST endpoints were added, along with integration to a PostgreSQL database in a container, simulating a basic app + database architecture.

> ⚠️ **Important:**  
> This project intentionally does not use `docker-compose`.  
> The goal is to first learn how to manage containers manually using commands like `docker build`, `docker run`, `docker network`, `docker volume`, etc., before progressing to abstraction tools like `docker-compose`.

## 🎯 Objective

Create a functional containerized environment to practice:

- Image creation and publishing to DockerHub  
- Local container execution  
- Custom Docker network configuration  
- Data persistence with volumes  
- Service parameterization via environment variables  
- Container and log monitoring/inspection  
- Accessing container shells for debugging and manual commands  
- Cleaning up the local Docker environment (containers, volumes, networks, images)

## ✅ Prerequisites

- .NET 8 SDK  
- Docker Desktop  
- Docker Hub account

## 🛠️ Full Step-by-Step Guide

### 🔹 1. Build the application image

```bash
docker build -f DockerPostgre/Dockerfile -t yourdockerhubuser/dockerpostgres:v1 .
