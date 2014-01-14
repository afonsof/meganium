#language: pt-br
Funcionalidade: Editar postagem
	Como administrador do site
	Gostaria de criar um objeto
	De modo que os usuários finais o visualize

Cenário: Editar postagem com sucesso
	Dado que estou logado
	E que o existe um tipo de objeto com todos os comportamentos
	E as seguintes postagens existem
	| Título              |
	| Receita de cup cake |
	Quando entro na página "/Admin/Post"
	E clico em "Editar" do item "Receita de cup cake"
	E limpo o campo "Título"
	E digito "Receita de cup cake gostoso" no campo "Título"
	E clico no botão "Salvar"
	Então estou na página "/Admin"
	E deu uma mensagem de sucesso
	E verifico que o seguinte item existe
	|  | Título                      |
	|  | Receita de cup cake gostoso |

Cenário: Editar postagem com campos vazios
	Dado que estou logado
	E que o existe um tipo de objeto com todos os comportamentos
	E as seguintes postagens existem
	| Título                |
	| Receita de pão de mel |
	Quando entro na página "/Admin/Post"
	E clico em "Editar" do item "Receita de pão de mel"
	E limpo o campo "Título"
	E clico no botão "Salvar"
	Então verifico uma mensagem de erro "Campo obrigatório" para o campo "Título"