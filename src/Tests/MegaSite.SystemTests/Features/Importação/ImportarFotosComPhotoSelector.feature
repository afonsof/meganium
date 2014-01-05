#language: pt-br
Funcionalidade: Importar Fotos com PhotoSelector
	Como administrador do site
	Gostaria de importar minhas fotos do meu computador
	De modo que os usuários finais as visualizem

Cenário: Importar Fotos do meu computador com sucesso
	Dado que estou logado
	E cliente "A e B" existe
	Quando entro na página "/Admin/Client"
	E clico em "Editar" do item "A e B"
	E insiro a imagem "bala-de-coco.jpg" 4 vezes
	E clico no link "Voltar para o topo"
	E clico no botão "Salvar"
	Então estou na página "/Admin/Client"
	E deu uma mensagem de sucesso
