# KML
A way to write Kustom presets in XML.


### How it works
1. Get **kml2kclip.exe** from the [releases tab](https://github.com/theothertored/kml/releases) of this repository.

2. Get **kml-schema.xsd** from [here](https://github.com/theothertored/kml/blob/master/kml2kclip/schema/kml-schema.xsd) and attach it to your XML editor of choice. Instructions for Visual Studio and VSCode below.

3. In your xml editor of choice, write a preset and save it in a file. I'd recommend using `.xml` as the extension, just so you don't have to manually bind `.kml` to an xml editor.

4. Run `kml2kclip.exe` from the console, passing the XML file as stdin or with the -in argument.
  - stdio: `kml2kclip <kmlfile.xml >kclip.txt`
  - args: `kml2kclip -in kmlfile.xml -out kclip.txt`  
  
    The program will compile your XML into a kustom clipboard JSON and put it in the file specified (or into the console, if no file was specified!)
   I would recommend against using `.json` as your output file extension, since it will include `##KUSTOMCLIP##` at the beginning and end, which will make it light up red in a JSON editor.

5. Put the compiled kustom clipboard JSON into your mobile device's clipboard. You can do this with Pushbullet, through a Google Keep note, whatever you want.

6. Go into a Kustom app of your choice and hit paste.

### How to attach schema

**VSCode**

1. Get the VSCode [XML by Red Hat](https://marketplace.visualstudio.com/items?itemName=redhat.vscode-xml) extension (you can easily find it by typing "xml" into the extension search box).

2. After you created your xml document, at the top of it, below `<?xml version="1.0"?>` put
```xml
<?xml-model href="D:\path\to\schema\kml-schema.xsd"?>
```
 Of course putting the correct schema path in the href attribute.

**Visual Studio**

1. Open the XML file you wish to edit.  

2.  In the properties window, select `Schemas` and click the `...` button.

3. Click `Add` on the right side, then select the `.xsd` file from your file system. If you added the file before, it should appear on the list, in that case, select its `Use` cell and select `Use this schema` from the dropdown. 


### How to get kml2kclip to run on `Ctrl + Shift + B` in VSCode (build task)

1. Open the folder containing your KML file in VSCode.

2. Hit `Ctrl + Shift + P` to open the Command Palette.

3. Find `Tasks: Configure Default Build Task`, hit enter, then enter again. From the list that pops up, select `Other`. VSCode will create `tasks.json` for you.

4. Edit the only task in the file:
 - the label can be whatever you want it to be
 - the type must be `shell`
 - the command should be `.\\kml2kclip -in kml-preset.xml -out kclip.txt` , or something similar.
    - `.\\` is there because of powershell
    - I had to make `-in` and `-out` work because of powershell too
    - why doesn't powershell support `<` and `>` for stdio???

5. After that, hit open the Command Palette again and execute `Tasks: Configure Default Build Task` again. Your task should pop up, press enter and you're good to go. From now on, `Ctrl + Shift + B` should compile the kml file.
