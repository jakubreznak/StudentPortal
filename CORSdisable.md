- pro registraci je nutné být ve vnitřní síti UP
 - dále je při registraci nutné mít vypnuté CORS, stačí provést jednu z následujících akcí, dle vašeho prohlížeče a OS:
	- Windows
	   = Google Chrome - [v adresáři kde se nachází Google Chrome], chrome.exe --user-data-dir="C://Chrome dev session" --disable-web-security
	
	   = Opera - [v adresáři kde se nachází Opera], launcher.exe --disable-web-security --user-data-dir="c:\nocorsbrowserdata"

	- Linux
	   = Google Chrome - google-chrome --disable-web-security

	- MacOs
	   = Google Chrome - open -a Google\ Chrome --args --disable-web-security --user-data-dir

	   = Safari - Enable the develop menu by going to Preferences > Advanced. Then select “Disable Cross-Origin Restrictions” from the develop menu.
