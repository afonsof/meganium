#language: pt-BR
Funcionalidade: Excluir Usuário
	Como administrador
	Gostaria de excluir um usuário
	De modo que outra pessoa possa me ajudar administrar o site

Cenário: Excluir usuário com sucesso
	Dado que estou logado
	E os seguintes usuários existem
	| Nome  | Nome de usuário | E-mail          |
	| João  | joao            | joao@gmail.com  |
	Quando entro na página "/Admin/User"
	E clico em "Excluir" do item "joao"
	E clico OK no alerta
	Então estou na página "/Admin/User"
	E deu uma mensagem de sucesso
	E os seguintes usuários não existem
	| Nome  | Nome de usuário | E-mail          |
	| João  | joao            | joao@gmail.com  |

Cenário: Excluir e cancelar a exclusão
	Dado que estou logado
	E os seguintes usuários existem
	| Nome  | Nome de usuário | E-mail          |
	| João  | joao            | joao@gmail.com  |
	Quando entro na página "/Admin/User"
	E clico em "Excluir" do item "joao"
	E clico Cancelar no alerta
	Então os seguintes usuários existem
	| Nome  | Nome de usuário | E-mail          |
	| João  | joao            | joao@gmail.com  |

Cenário: Excluir usuário que tem posts cadastrados
	Dado que estou logado
	E os seguintes usuários existem
	| Nome           | Nome de usuário | E-mail          |
	| Mario Postador | mario           | mario@gmail.com |
	E o usuário "mario" tem um post
	Quando entro na página "/Admin/User"
	E clico em "Excluir" do item "mario"
	E clico OK no alerta
	Então verifico uma mensagem de erro global "Não pode ser excluído pois este usuário tem objetos cadastrados em seu nome"
	E os seguintes usuários existem
	| Nome           | Nome de usuário | E-mail          |
	| Mario Postador | mario           | mario@gmail.com |