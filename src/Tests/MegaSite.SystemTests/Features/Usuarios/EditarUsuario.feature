#language: pt-BR
Funcionalidade: Editar Usuário
	Como administrador
	Gostaria de editar um usuário
	De modo que outra pessoa possa me ajudar administrar o site

Cenário: Editar usuario com campos vazios
	Dado que estou logado
	E os seguintes usuários existem
	| Nome  | Nome de usuário | E-mail          |
	| João  | joao            | joao@gmail.com  |
	Quando entro na página "/Admin/User"
	E clico em "Editar" do item "joao"
	E limpo o campo "FullName"
	E clico no botão "Salvar"
	Então verifico uma mensagem de erro "Campo obrigatório" para o campo "FullName"

Cenário: Editar usuário com sucesso
	Dado que estou logado
	E os seguintes usuários existem
	| Nome  | Nome de usuário | E-mail          |
	| João  | joao            | joao@gmail.com  |
	Quando entro na página "/Admin/User"
	E clico em "Editar" do item "joao"
	E limpo o campo "FullName"
	E digito "João Editado" no campo "FullName"
	E clico no botão "Salvar"
	Então estou na página "/Admin/User"
	E deu uma mensagem de sucesso
	E verifico que o seguinte item existe
	| Nome         | Nome de usuário | E-mail         |
	| João Editado | joao            | joao@gmail.com |