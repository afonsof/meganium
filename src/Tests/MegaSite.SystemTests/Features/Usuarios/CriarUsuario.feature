#language: pt-BR
Funcionalidade: Criar Usuário
	Como administrador
	Gostaria de criar um usuário
	De modo que outra pessoa possa me ajudar administrar o site

Cenário: Criar usuário com sucesso
	Dado que estou logado
	E usuário "jose" não existe
	Quando entro na página "/Admin/User/Create"
	E digito "Jose Silva" no campo "FullName"
	E digito "jose" no campo "UserName"
	E digito "jose@gmail.com" no campo "Email"
	E digito "123" no campo "Password"
	E clico no botão "Salvar"
	Então estou na página "/Admin/User"
	E deu uma mensagem de sucesso
	E verifico que o seguinte item existe
	| Nome       | Nome de usuário | E-mail         |
	| Jose Silva | jose            | jose@gmail.com |

Cenário: Criar usuario com campos vazios
	Dado que estou logado
	Quando entro na página "/Admin/User/Create"
	E clico no botão "Salvar"
	Então estou na página "/Admin/User/Create"
	E verifico uma mensagem de erro "Campo obrigatório" para o campo "FullName"
	E verifico uma mensagem de erro "Campo obrigatório" para o campo "UserName"
	E verifico uma mensagem de erro "Campo obrigatório" para o campo "Email"
	E verifico uma mensagem de erro "Campo obrigatório" para o campo "Password"

Cenário: Criar usuario com dados inválidos
	Dado que estou logado
	Quando entro na página "/Admin/User/Create"
	E digito "www.jose.com" no campo "Email"
	E clico no botão "Salvar"
	Então estou na página "/Admin/User/Create"
	E verifico uma mensagem de erro "E-mail Inválido" para o campo "Email"

Cenário: Criar usuário com email duplicado
	Dado que estou logado
	E os seguintes usuários existem
	| Nome  | Nome de usuário | E-mail          |
	| Maria | maria           | maria@gmail.com |
	Quando entro na página "/Admin/User/Create"
	E digito "Maria Santos" no campo "FullName"
	E digito "mariasantos" no campo "UserName"
	E digito "maria@gmail.com" no campo "Email"
	E digito "123" no campo "Password"
	E clico no botão "Salvar"
	Então estou na página "/Admin/User/Create"
	E verifico uma mensagem de erro global "O usuário não pode ser salvo pois já existe alguém cadastrado com este e-mail"
	E o valor do campo "FullName" é "Maria Santos"
	E o valor do campo "UserName" é "mariasantos"
	E o valor do campo "Email" é "maria@gmail.com"
	E o valor do campo "Password" está vazio

Cenário: Criar usuário com username duplicado
	Dado que estou logado
	E os seguintes usuários existem
	| Nome  | Nome de usuário | E-mail          |
	| Maria | maria           | maria@gmail.com |
	Quando entro na página "/Admin/User/Create"
	E digito "Maria Mendes" no campo "FullName"
	E digito "maria" no campo "UserName"
	E digito "mariamendes@gmail.com" no campo "Email"
	E digito "123" no campo "Password"
	E clico no botão "Salvar"
	Então estou na página "/Admin/User/Create"
	E verifico uma mensagem de erro global "O usuário não pode ser salvo pois já existe alguém cadastrado com este nome de usuário"
	E o valor do campo "FullName" é "Maria Mendes"
	E o valor do campo "UserName" é "maria"
	E o valor do campo "Email" é "mariamendes@gmail.com"
	E o valor do campo "Password" está vazio