#language: pt-br
Funcionalidade: Criar tipo de objeto
	Como um dono do sistema
	Gostaria de criar um tipo de objeto
	De modo que adapte o sistema às necessidades do cliente

Cenário: Criar tipo de objeto
	Dado que estou logado
	Quando entro na página "/Admin/postType/Create"
	E digito "Objeto" no campo "SingularName"
	E digito "Objetos" no campo "PluralName"
	E digito "book" no campo "IconId"
	E clico no botão "Salvar"
	Então estou na página "/Admin/postType"
	E verifico que o seguinte item existe
	| Nome no singular | Nome no plural |
	| Objeto           | Objetos        |

Cenário: Criar tipo de objeto com campos vazios
	Dado que estou logado
	Quando entro na página "/Admin/postType/Create"
	E clico no botão "Salvar"
	Então estou na página "/Admin/postType/Create"
	E verifico uma mensagem de erro "Campo obrigatório" para o campo "SingularName"
	E verifico uma mensagem de erro "Campo obrigatório" para o campo "PluralName"
	E verifico uma mensagem de erro "Campo obrigatório" para o campo "IconId"