#language: pt-br
Funcionalidade: Importar Videos do Youtube
	Como administrador do site
	Gostaria de importar meus vídeos do Youtube
	De modo que os usuários finais os visualizem

Cenário: Importar Videos do Youtube com sucesso
	Dado que estou logado
	Quando entro na página "/Admin/Import"
	E seleciono "Vídeos do Youtube" no campo "PluginName"
	E digito "afonsof" no campo "username"
	E clico no botão "Avançar"
	E clico no botão "Importar tudo"
	Então estou na página "/Admin/Import"
	E deu uma mensagem de sucesso