#language: pt-br
Funcionalidade: Gerenciar Destaques
	Como administrador do site
	Gostaria de ajustar os objetos que serão destaque no site
	De modo que os usuários finais saibam o que há de mais importante no site

Cenário: Gerenciar destaques com sucesso
	Dado que estou logado
	E que o existe um tipo de objeto com todos os comportamentos
	E as seguintes postagens existem
	| Título                 | Destaque |
	| Receita de Muffin Azul | Sim      |
	Quando entro na página "/Admin/Featured/Manage"
	E limpo a seleção da caixa de multiseleção
	E seleciono os itens na caixa de multiseleção
	| Título                 |
	| Receita de Muffin Azul |
	E clico no botão "Salvar"
	Então estou na página "/Admin/Featured"
	E deu uma mensagem de sucesso
	E verifico que o seguinte item existe
	| Título                 |
	| Receita de Muffin Azul |