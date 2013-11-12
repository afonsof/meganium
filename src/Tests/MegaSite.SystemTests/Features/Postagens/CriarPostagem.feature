#language: pt-br
Funcionalidade: Criar postagem
	Como administrador do site
	Gostaria de criar um objeto
	De modo que os usuários finais o visualize

Cenário: Criar postagem inserindo imagem com sucesso
	Dado que estou logado
	E que o existe um tipo de objeto com todos os comportamentos
	Quando entro na página "/Admin/Post/Create"
	E digito "Receita de bala de coco" no campo "Título"
	E insiro a imagem "bala-de-coco.jpg"
	E clico no botão "Salvar"
	Então estou na página "/Admin"
	E deu uma mensagem de sucesso
	E verifico que o seguinte item existe
	|                  | Título                  |
	| bala-de-coco.jpg | Receita de bala de coco |

Cenário: Criar postagem com campos vazios
	Dado que estou logado
	E que o existe um tipo de objeto com todos os comportamentos
	Quando entro na página "/Admin/Post/Create"
	E clico no botão "Salvar"
	Então estou na página "/Admin/Post/Create"
	E verifico uma mensagem de erro "Campo obrigatório" para o campo "Título"

