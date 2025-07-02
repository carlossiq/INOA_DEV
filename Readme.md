# API: BRAPI
  ## Token:
    iZZruX5xyKkXKj1f1kh21Z

# Execução:
    dotnet run -- ATIVO PRECO_VENDA PRECO_COMPRA
    stock-quote-alert.exe ATIVO PRECO_VENDA PRECO_COMPRA
    
    ex1: dotnet run -- PETR4 40.00 35.00
    ex2: stock-quote-alert.exe VIVT3 40.00 35.00

# Utilização do gmail:
    - O usuário deve ativar autenticação em duas etapas;
    - Criar a senha de aplicativo:
      https://myaccount.google.com/apppasswords

# Configuração do JSON
  ## Language:
    Escolher entre: pt, en
    Default: en

  ## Arquivo:
    - Atualizar exemplo_config.json para config.json com as informações corretas.

# About
  O aplicativo possui suporte en/pt;
  As requisições são enviadas a cada minuto, apesar da BRAPI só modificar os dados a cada 30 minutos;
  Caso haja um bloqueio por excesso de requisições, o app espera 5 minutos para enviar a próxima requisição e aumenta o intervalo de requisições em até 10 minutos;
  Os e-mails são enviados somente quando há mudança de estado (buy, hold, sell);

  Foi usado um template dinâmico para estilizar os e-mails gerados;
  AtivoService.cs é responsável por fazer as requisições na API;
  EmailService.cs é responsável por enviar os e-mails; 