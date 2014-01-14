#language: pt-br
Funcionalidade: Importar Fotos do Facebook
	Como administrador do site
	Gostaria de importar minhas fotos do Facebook
	De modo que os usuários finais as visualizem

Cenário: Importar Fotos do Facebook com sucesso
	Dado que estou logado
	Quando entro na página "/Admin/Import"
	E seleciono "Fotos do Facebook" no campo "PluginName"
	E clico no botão "Avançar"
	E digito "meganiumteste@gmail.com" no campo "email"
	E digito "123mudar*" no campo "pass"
	E clico no botão "login"
	E clico no botão "Importar tudo"
	Então estou na página "/Admin/Import"
	E deu uma mensagem de sucesso