# KML
A way to write Kustom presets in XML.


### How it works
1. In your xml editor of choice, write a preset (you can import schema to make this step easier).
2. Run `kml2kustomclip.exe`, passing the XML file into stdin (example: `kml2kustomclip.exe <kmlexample.xml >kustomclip.txt`). The program will compile your XML into a kustom clipboard JSON.
3. Put the compiled kustom clipboard JSON into your mobile device's clipboard.
4. Go into a Kustom app of your choice and hit paste.
