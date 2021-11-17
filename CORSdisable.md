- pro registraci je nutné být ve vnitřní síti UP
 - dále je při registraci nutné mít vypnuté CORS, stačí provést jednu z následujících akcí, dle vašeho prohlížeče a OS:
	- Windows
	   = Google Chrome - [v adresáři kde se nachází Google Chrome], chrome.exe --user-data-dir="C://Chrome dev session" --disable-web-security
	
	   = Opera - [v adresáři kde se nachází Opera], launcher.exe --disable-web-security --user-data-dir="c:\nocorsbrowserdata"

	- Linux
	   = Google Chrome - google-chrome --disable-web-security

	- MacOs
	   = Safari - Preferences -> Advanced -> Show Develop menu in bar, Otevřít Develop menu -> Označit “Disable Cross-Origin Restrictions”. Znovu načíst stránku!
