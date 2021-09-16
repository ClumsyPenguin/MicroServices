# MicroServices

[![Codacy Badge](https://api.codacy.com/project/badge/Grade/194cb58c05014bf8a46c770a2fb6e438)](https://app.codacy.com/gh/ClumsyPenguin/MicroServices?utm_source=github.com&utm_medium=referral&utm_content=ClumsyPenguin/MicroServices&utm_campaign=Badge_Grade_Settings)

## Prerequisites
-  Docker Desktop with WSL2 enabled
-  Kubernetes `kubectl`
-   .NET5 Runtime

### Deployment Guide
1. Clone/Fork repo
2. Open K8S folder 
3. Run all the deployments on your local machine `kubectl create deployment <FileName>`
4. Go to `C:\Windows\System32\drivers\etc\hosts` and add on a blank line `127.0.0.0.1 acme.com`
5. Congrats your kubernetes network is up and running

For now every service has its own Pod.

![Capture](https://user-images.githubusercontent.com/22469147/133261406-47943723-ccc2-4527-9279-1263e2c7a862.PNG)

You can test calls both with the defined node port number or Nginx:

`https://acme.com/api/Platforms`

`http://localhost:<port>/api/Platforms`

#### Dockerhub links
CommandService: https://hub.docker.com/repository/docker/clumpsypenguin/commandservice

PlatformService: https://hub.docker.com/repository/docker/clumpsypenguin/platformservice

#### Todo's
- Implement a messagebus *ex. RabbitMQ*
- Replace in memory DB with a SQL server one
- Tweaking resources needed for kubernetes network

