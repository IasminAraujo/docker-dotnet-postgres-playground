# 🐳 API .NET 8 + PostgreSQL com Docker: Estrutura de Teste de Comandos

> 🇺🇸 Looking for the English version? [Click here](README.en.md)

Este repositório apresenta uma base de testes voltada para desenvolvedores que desejam **praticar comandos essenciais do Docker** aplicados a uma aplicação ASP.NET Core com PostgreSQL.

A aplicação foi gerada com o **template padrão do Visual Studio para APIs .NET 8**, o qual já inclui um `Dockerfile` pronto para uso. Foram adicionados **endpoints simples de GET e POST** e feita a integração com um **banco PostgreSQL em container**, permitindo simular uma arquitetura básica de aplicação + banco.

> ⚠️ Importante:
> 
> 
> Este projeto **não utiliza `docker-compose` de propósito**.
> 
> A proposta é aprender primeiro os **comandos manuais do Docker** (`docker build`, `docker run`, `docker network`, `docker volume`, etc.), para depois evoluir naturalmente para ferramentas de abstração como o `docker-compose`.
> 

---

## 🎯 Objetivo

- Criar um ambiente containerizado funcional para prática de:
    - Criação de imagem e publicação no DockerHub
    - Execução local de containers
    - Configuração de rede Docker personalizada
    - Persistência de dados com volumes
    - Parametrização de serviços via variáveis de ambiente
    - Monitoramento e inspeção dos containers e logs
    - Acesso ao shell dos containers para debug e comandos manuais
    - Limpeza do ambiente Docker local (containers, volumes, redes e imagens)

---

## ✅ Pré-requisitos

- .NET 8 SDK
- Docker Desktop
- Conta no [Docker Hub](https://hub.docker.com/)

---

## 🛠️ Passo a passo completo

### 🔹 1. Construir a imagem da aplicação

```bash
docker build -f DockerPostgre/Dockerfile -t nomerepositoriodockerhub/dockerpostgres:v1 .
```

📌 **Explicação passo a passo**:

- `docker build`: comando que cria uma imagem Docker a partir de um `Dockerfile`.
- `f DockerPostgre/Dockerfile`: especifica o caminho para o `Dockerfile`. Neste caso, ele foi gerado automaticamente pelo template do Visual Studio e está dentro da pasta `DockerPostgre`.
- `t nomerepositoriodockerhub/dockerpostgres:v1`: define o nome da imagem no padrão `<usuário ou organização do Docker Hub>/<nome da imagem>:<tag>`.
    - **Importante**: como a imagem será publicada no Docker Hub, o nome **precisa começar com o nome do repositório (usuário ou organização)**.
    - Substitua `nomerepositoriodockerhub` pelo seu usuário no DockerHub, por exemplo: `appdotnetpostgres`.
- `.`: indica que o contexto de build (ou seja, os arquivos usados para montar a imagem) é o diretório atual.

✅ **Boas práticas**:

- Sempre use um nome que reflita o destino da imagem no Docker Hub.
- Inclua uma `tag` como `v1`, `v2`, etc., para versionar suas builds. Isso facilita rollback e controle de ambientes.

---

### 🔹 2. Fazer login e publicar a imagem versionada no Docker Hub

Antes de publicar qualquer imagem, você precisa estar autenticado com sua conta no Docker Hub.

### 👉 2.1 Faça login na sua conta Docker

```bash
docker login
```

📌 **Explicação**:

- Esse comando solicitará seu **usuário e senha** (ou token de acesso).
- Após o login, você poderá enviar imagens para repositórios públicos ou privados vinculados à sua conta.

---

### 👉 2.2 Faça o push da imagem versionada (`v1`)

```bash
docker push nomerepositoriodockerhub/dockerpostgres:v1
```

📌 **Explicação**:

- Este comando publica a imagem que você construiu localmente no repositório remoto `nomerepositoriodockerhub/dockerpostgres` com a tag `v1`.
- Certifique-se de que a imagem foi construída corretamente antes deste passo (veja o passo anterior).

✅ **Boas práticas**:

- Sempre utilize uma **tag versionada**, como `v1`, `v2`, `v1.0.1`, etc.
- Isso garante rastreabilidade, rollback e controle de deploys.

---

### 🔹 3. Criar e publicar a tag `latest` da imagem

Este passo é opcional, mas altamente recomendável em ambientes de desenvolvimento.

### 👉 3.1 Criar uma nova tag `latest` a partir da imagem já existente

```bash
docker tag nomerepositoriodockerhub/dockerpostgres:v1 nomerepositoriodockerhub/dockerpostgres:latest
```

📌 **Explicação**:

- Esse comando **não cria uma nova imagem**, apenas **associa um novo rótulo** (`latest`) à imagem que já foi construída.
- Ele aponta `latest` para o mesmo conteúdo de `v1`.

✅ **Você pode confirmar com:**

```bash
docker images ls
```

🧠 Você verá que as tags `v1` e `latest` compartilham o **mesmo IMAGE ID**. Isso significa que elas são tecnicamente a mesma imagem com nomes diferentes.

---

### 👉 3.2 Fazer push da tag `latest` para o Docker Hub

```bash
docker push nomerepositoriodockerhub/dockerpostgres:latest
```

📌 **Por que criar a tag `latest`?**

- A tag `latest` é uma **convenção** para indicar a versão mais recente da imagem.
- Pode ser usada por times, pipelines ou testes que não precisam de uma versão fixa.
- Facilita o consumo da imagem com comandos como:

```bash
docker pull nomerepositoriodockerhub/dockerpostgres
```

❗ **Atenção**: em ambientes de produção, o ideal é sempre referenciar **tags específicas** para garantir controle de versão.

---

### 🔹 4. Criar um volume Docker para persistência de dados

```bash
docker volume create app_postgres_vol
```

📌 **Explicação**:

Esse comando cria um volume nomeado chamado `app_postgres_vol`, que será usado para armazenar os dados do PostgreSQL de forma persistente.

---

### 🧠 Por que usar volumes?

Por padrão, tudo que acontece dentro de um container é **transitório**. Em outras palavras:

> Containers são efêmeros: quando você os remove ou recria, todos os dados internos são perdidos.
> 

Ao usar um **volume externo**, você:

- Garante que os dados armazenados (como os do banco PostgreSQL) sobrevivam ao ciclo de vida dos containers;
- Pode parar, recriar ou atualizar os containers sem perder informações importantes;
- Separa o armazenamento da lógica de execução.

---

### 🔹 5. Criar uma rede Docker personalizada

```bash
docker network create app_postgres_net
```

📌 **Explicação**:

Esse comando cria uma **rede Docker personalizada** chamada `app_postgres_net`. Ela será usada para conectar os containers da aplicação e do banco de dados, permitindo que eles se comuniquem de forma isolada e controlada.

---

### 🧠 Por que usar uma rede Docker?

Por padrão, o Docker isola os containers em redes distintas. Se você **não explicita uma rede comum**, os containers não conseguem se comunicar entre si **por nome** — apenas via IPs, que são **dinâmicos e não confiáveis** em ambientes efêmeros.

---

### ✅ Por que criar uma bridge personalizada?

O Docker já cria uma rede `bridge` por padrão (`bridge`), mas:

- Criar uma **rede nomeada personalizada** (como `app_postgres_net`) traz mais **controle e clareza**.
- Você pode facilmente **inspecionar os containers conectados a ela**.
- Simula ambientes reais onde serviços se comunicam por nomes DNS internos.

> Em produção, ferramentas como Docker Compose e Kubernetes criam redes virtuais isoladas para facilitar a comunicação entre serviços — aqui estamos fazendo isso manualmente, por aprendizado.
> 

---

### 📡 Benefício direto:

Ao conectar os containers à mesma rede (`app_postgres_net`), você poderá configurar a string de conexão do PostgreSQL usando o **nome do container**, assim:

```
Host=db_postgres
```

Isso elimina a necessidade de descobrir e usar IPs, o que **aumenta a portabilidade e estabilidade da configuração**.

---

### 🔹 6. Criar e executar o container do PostgreSQL

```bash
docker container run -d \
  -p 5432:5432 \
  --name db_postgres \
  -e POSTGRES_PASSWORD=Pg123 \
  -e POSTGRES_USER=usuario \
  -e POSTGRES_DB=app_postgres \
  --network app_postgres_net \
  -v app_postgres_vol:/var/lib/postgresql/data \
  postgres:14.18
```

---

### 🧠 O que está acontecendo aqui:

| Trecho do comando | Explicação |
| --- | --- |
| `docker container run -d` | Cria e executa o container em segundo plano (modo *detached*). |
| `-p 5432:5432` | Mapeia a porta padrão do PostgreSQL (5432) do container para o host, permitindo conexões externas. |
| `--name db_postgres` | Dá um nome ao container, o que facilita comandos futuros e comunicação entre containers pela rede. |
| `-e POSTGRES_PASSWORD=Pg123` | Define a senha do usuário principal do PostgreSQL. **Obrigatório** para subir o container. |
| `-e POSTGRES_USER=usuario` | Define o nome do usuário principal do PostgreSQL. Opcional (o default seria `postgres`). |
| `-e POSTGRES_DB=app_postgres` | Cria automaticamente um banco de dados com esse nome na inicialização. |
| `--network app_postgres_net` | Conecta o container à rede customizada criada anteriormente, permitindo que outros containers o encontrem por `nome`. |
| `-v app_postgres_vol:/var/lib/postgresql/data` | Monta o volume criado no local onde o PostgreSQL armazena seus dados. |
| `postgres:14.18` | Imagem oficial do PostgreSQL na versão 14.18, baixada do Docker Hub. |

---

### 📁 Por que `/var/lib/postgresql/data`?

Esse é o **caminho interno padrão** utilizado pela imagem oficial do PostgreSQL para armazenar os dados do banco. Ao montar nosso volume nesse caminho, garantimos que:

> ✅ Os dados não fiquem dentro do container, mas sim no volume externo — o que permite que sobrevivam mesmo após a exclusão ou recriação do container.
> 

---

### 📚 Sobre as variáveis de ambiente

A imagem oficial do PostgreSQL aceita várias variáveis de ambiente que controlam a configuração inicial do banco. As mais comuns são:

- `POSTGRES_PASSWORD` (obrigatória);
- `POSTGRES_USER`;
- `POSTGRES_DB`.

Para ver a lista completa e atualizada de variáveis, consulte:

🔗 https://hub.docker.com/_/postgres

---

### 🧪 Testando a conexão com o banco de dados

Depois que o container estiver rodando, é altamente recomendável validar a conexão com o PostgreSQL. Você pode usar qualquer uma das ferramentas abaixo:

### GUI:

- **DBeaver** – multiplataforma e excelente para uso local.
- **pgAdmin** – interface gráfica oficial do PostgreSQL.

### Linha de comando:

Se você tiver o cliente `psql` instalado:

```bash
psql -h localhost -p 5432 -U usuario -d app_postgres
```

> A senha será Pg123, conforme definida na variável de ambiente.
> 

⚠️ **Importante:**

Após o banco de dados estar operacional, aplique as migrations da aplicação para garantir que a estrutura esteja criada corretamente

---

### 🔹 7. Rodar o container da aplicação na mesma rede do banco

```bash
docker container run -d \
  -p 8080:8080 \
  -e ASPNETCORE_ENVIRONMENT=Development \
  --name app_postgres \
  --network app_postgres_net \
  nomerepositoriodockerhub/dockerpostgres:latest
```

---

### 🧠 O que está acontecendo aqui:

| Trecho do comando | Explicação |
| --- | --- |
| `docker container run -d` | Executa a aplicação em segundo plano (modo *detached*). |
| `-p 8080:8080` | Mapeia a porta da aplicação do container (`8080`, definida no Dockerfile) para a porta do host, permitindo acessos locais como `http://localhost:8080`. |
| `-e ASPNETCORE_ENVIRONMENT=Development` | Define a variável de ambiente usada pelo ASP.NET Core. Em modo `Development`, o Swagger fica habilitado por padrão. |
| `--name app_postgres` | Dá um nome fixo ao container, facilitando inspeção e gerenciamento. |
| `--network app_postgres_net` | Conecta este container à mesma rede `app_postgres_net` onde o PostgreSQL já está, permitindo que a aplicação acesse o banco pelo nome `db_postgres`. |
| `nomerepositoriodockerhub/dockerpostgres:latest` | Usa a imagem da aplicação publicada anteriormente no Docker Hub, com a tag `latest`. |

---

### ✅ Confirmar que os containers estão na mesma rede

```bash
docker network inspect app_postgres_net
```

📌 **Explicação**:

- Este comando mostra todos os containers conectados à rede `app_postgres_net`.
- Você deve ver tanto `app_postgres` quanto `db_postgres` listados.
- Isso confirma que a comunicação interna está funcionando via DNS Docker.

---

### 🔗 Comunicação entre aplicação e banco

Como ambos os containers estão na mesma **rede bridge personalizada**, a string de conexão da aplicação pode usar o **nome do container do PostgreSQL (`db_postgres`) como host**, como neste exemplo:

```
Host=db_postgres;Port=5432;Username=usuario;Password=Pg123;Database=app_postgres;
```

> ✅ Essa abordagem elimina dependência de IPs fixos, o que é essencial em ambientes com containers efêmeros.
> 

---

### 🔹 8. Monitorar containers e acessar shell para debug

Para acompanhar o status dos containers e ajudar no diagnóstico:

- **Listar containers em execução:**

```bash
docker ps
```

- **Ver logs dos containers (útil para entender erros ou comportamento):**

```bash
docker logs app_postgres
docker logs db_postgres
```

- **Acessar o terminal do container do banco para inspeção e comandos manuais:**

```bash
docker exec -it db_postgres bash
```

> Essa prática é fundamental para entender o estado interno do banco e realizar comandos SQL direto no container.
> 

---

### 🔹 9. Limpar ambiente Docker local

Após terminar os testes, é recomendável limpar containers, volumes e redes para evitar acúmulo e conflitos.

```bash
docker container stop app_postgres db_postgres
docker container rm app_postgres db_postgres
docker volume rm app_postgres_vol
docker network rm app_postgres_net
docker image rm nomerepositoriodockerhub/dockerpostgres:v1 nomerepositoriodockerhub/dockerpostgres:latest

```

> Dica: manter o ambiente Docker organizado é essencial para produtividade e estabilidade em projetos reais.
> 

---

### 🔹 10. Testar a API

- Use o navegador para acessar `http://localhost:8080/swagger` e testar os endpoints.
- O endpoint de POST deve inserir dados no banco PostgreSQL, e o de GET deve consultá-los.

### 🚀 Encerramento

Dominar comandos manuais do Docker é um passo importante para qualquer desenvolvedor que quer garantir controle fino sobre seus ambientes. Essa base vai preparar você para avançar a orquestração com ferramentas como `docker-compose` e Kubernetes, e atuar com maior senioridade em times modernos de desenvolvimento.
