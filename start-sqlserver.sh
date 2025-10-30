
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
PURPLE='\033[0;35m'
CYAN='\033[0;36m'
NC='\033[0m'

echo -e "${BLUE}Iniciando SQL Server no GitHub Codespaces...${NC}"

command_exists() {
    command -v "$1" >/dev/null 2>&1
}

echo -e "${CYAN}Verificando Docker...${NC}"
if ! docker info > /dev/null 2>&1; then
    echo -e "${RED}Docker não está rodando.${NC}"
    echo -e "${YELLOW}Tentando iniciar Docker...${NC}"

    if command_exists "sudo"; then
        sudo service docker start
        sleep 5
    else
        echo -e "${RED}Não foi possível iniciar Docker automaticamente.${NC}"
        echo -e "${YELLOW}Execute manualmente: sudo service docker start${NC}"
        exit 1
    fi

    if ! docker info > /dev/null 2>&1; then
        echo -e "${RED}Docker ainda não está rodando.${NC}"
        echo -e "${YELLOW}Verifique se o Docker está instalado e tente novamente.${NC}"
        exit 1
    fi
fi

echo -e "${GREEN}Docker está rodando!${NC}"

if ! command_exists "docker-compose"; then
    echo -e "${YELLOW}docker-compose não encontrado, tentando usar 'docker compose'...${NC}"
    if command_exists "docker" && docker compose version > /dev/null 2>&1; then
        COMPOSE_CMD="docker compose"
    else
        echo -e "${RED}Nem docker-compose nem 'docker compose' estão disponíveis.${NC}"
        echo -e "${YELLOW}Instale docker-compose ou atualize o Docker.${NC}"
        exit 1
    fi
else
    COMPOSE_CMD="docker-compose"
fi

echo -e "${CYAN}Verificando containers existentes...${NC}"
if docker ps --format "table {{.Names}}" | grep -q "biblioteca-sqlserver"; then
    echo -e "${YELLOW}Container 'biblioteca-sqlserver' já está rodando!${NC}"
    echo -e "${BLUE}Status atual:${NC}"
    docker ps | grep biblioteca-sqlserver

    echo -e "${CYAN}Deseja reiniciar o container? (y/N)${NC}"
    read -r response
    if [[ "$response" =~ ^[Yy]$ ]]; then
        echo -e "${YELLOW}Parando container existente...${NC}"
        $COMPOSE_CMD down
    else
        echo -e "${GREEN}Usando container existente.${NC}"
        SKIP_START=true
    fi
else
    echo -e "${GREEN}Nenhum container conflitante encontrado.${NC}"
    SKIP_START=false
fi

if [ "$SKIP_START" != "true" ]; then
    echo -e "${YELLOW}Parando containers existentes...${NC}"
    $COMPOSE_CMD down
fi

if [ "$SKIP_START" != "true" ]; then
    echo -e "${BLUE}Iniciando SQL Server container...${NC}"
    $COMPOSE_CMD up -d

    echo -e "${CYAN}Aguardando SQL Server ficar pronto...${NC}"
    echo -e "${YELLOW}   (Isso pode levar até 60 segundos na primeira execução)${NC}"

    for i in {1..30}; do
        echo -n "."
        sleep 2

        if docker exec biblioteca-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P YourStrong@Passw0rd -Q "SELECT 1" > /dev/null 2>&1; then
            echo ""
            echo -e "${GREEN}SQL Server está respondendo!${NC}"
            break
        fi

        if [ $i -eq 30 ]; then
            echo ""
            echo -e "${YELLOW}SQL Server pode ainda estar inicializando...${NC}"
        fi
    done
fi

if docker ps | grep -q "biblioteca-sqlserver"; then
    echo -e "${GREEN}SQL Server está rodando!${NC}"
    echo -e "${BLUE}Status do container:${NC}"
    docker ps | grep biblioteca-sqlserver

    echo ""
    echo -e "${PURPLE}Connection String:${NC}"
    echo "Server=localhost,1433;Database=BibliotecaUniversitaria;User Id=sa;Password=YourStrong@Passw0rd;MultipleActiveResultSets=true;TrustServerCertificate=true"

    echo ""
    echo -e "${CYAN}Próximos passos:${NC}"
    echo -e "${YELLOW}1. Execute as migrations:${NC}"
    echo "   dotnet tool restore"


    echo ""
    echo -e "${CYAN}Próximos passos:${NC}"
    echo -e "${YELLOW}2. Execute as migrations:${NC}"
    echo "   dotnet ef database update --project BibliotecaUniversitaria.Infrastructure --startup-project BibliotecaUniversitaria.Presentation"
    echo ""
    echo -e "${YELLOW}3. Execute a aplicação:${NC}"
    echo "   dotnet run --project BibliotecaUniversitaria.Presentation"

    echo ""
    echo -e "${PURPLE}Dicas úteis:${NC}"
    echo -e "${YELLOW}• Se 'dotnet ef' não funcionar, instale as ferramentas:${NC}"
    echo "   dotnet tool install --global dotnet-ef"
    echo ""
    echo -e "${YELLOW}• Para ver logs do SQL Server:${NC}"
    echo "   docker logs biblioteca-sqlserver"
    echo ""
    echo -e "${YELLOW}• Para parar o SQL Server:${NC}"
    echo "   $COMPOSE_CMD down"
    echo ""
    echo -e "${YELLOW}• Para reiniciar o SQL Server:${NC}"
    echo "   $COMPOSE_CMD restart"

    echo ""
    echo -e "${GREEN}Tudo pronto! Acesse http://localhost:5000${NC}"
else
    echo -e "${RED}Erro ao iniciar SQL Server.${NC}"
    echo -e "${YELLOW}Logs do container:${NC}"
    $COMPOSE_CMD logs sqlserver

    echo ""
    echo -e "${PURPLE}Soluções possíveis:${NC}"
    echo -e "${YELLOW}• Verifique se a porta 1433 está livre:${NC}"
    echo "   netstat -tulpn | grep 1433"
    echo ""
    echo -e "${YELLOW}• Tente reiniciar o Docker:${NC}"
    echo "   sudo service docker restart"
    echo ""
    echo -e "${YELLOW}• Verifique os logs detalhados:${NC}"
    echo "   docker logs biblioteca-sqlserver --tail 50"
fi
