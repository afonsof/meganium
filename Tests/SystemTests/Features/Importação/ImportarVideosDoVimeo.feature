#language: pt-br
Funcionalidade: Importar Videos do Vimeo
	Como administrador do site
	Gostaria de importar meus vídeos do Vimeo
	De modo que os usuários finais os visualizem

Cenário: Importar Videos do Vimeo com sucesso
	Dado que estou logado
	Quando entro na página "/Admin/Import"
	E seleciono "Vídeos do Vimeo" no campo "PluginName"
	E digito "sonhartphotoevideo" no campo "username"
	E clico no botão "Avançar"
	E clico no botão "Importar tudo"
	Então estou na página "/Admin/Import"
	E deu uma mensagem de sucesso