#language: pt-br
Funcionalidade: Criar postagem
	Como administrador do site
	Gostaria de criar um objeto
	De modo que os usuários finais o visualize

Cenário: Criar postagem com sucesso
	Dado que estou logado
	E que o existe um tipo de objeto com todos os comportamentos
	Quando entro na página "/Admin/Post/Create"
	E digito "Postagem inicial" no campo "Título"
	E clico no botão "Salvar"
	Então estou na página "/Admin"
	E deu uma mensagem de sucesso
	E verifico que o seguinte item existe
	|  | Título           |
	|  | Postagem inicial |

Cenário: Criar postagem com campos vazios
	Dado que estou logado
	E que o existe um tipo de objeto com todos os comportamentos
	Quando entro na página "/Admin/Post/Create"
	E clico no botão "Salvar"
	Então estou na página "/Admin/Post/Create"
	E verifico uma mensagem de erro "Campo obrigatório" para o campo "Título"

