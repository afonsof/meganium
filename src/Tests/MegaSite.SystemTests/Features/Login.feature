#language: pt-br
Funcionalidade: Login
	Como um usuário comum
	Gostaria de efetuar login
	Para utilizar o sistema

Cenário: Login através do acesso de uma página interna
	Dado que estou deslogado
	E entro na página "/Admin"
	E sou redirecionado para a tela de login
	Quando digito "teste@megasiteapp.com.br" no campo "Email"
	E digito "teste123" no campo "Password"
	E clico no botão "Entrar"
	Então estou na página "/Admin"

Cenário: Login pela tela de login
	Dado que estou deslogado
	E entro na página "/Admin"
	E sou redirecionado para a tela de login
	Quando digito "teste@megasiteapp.com.br" no campo "Email"
	E digito "teste123" no campo "Password"
	E clico no botão "Entrar"
	Então estou na página "/Admin"
