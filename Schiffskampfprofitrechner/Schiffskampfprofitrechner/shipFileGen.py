# Ship File Generator
# This tool will automatically read through a provided textfile and, after stripping whitespaces and dashes, create a .cs Class file using that name.
# REDUNDANT
import os

def clearFolder(folder):
    for fil in os.listdir(folder): 
        abs_path = os.path.join(folder, fil)
        try:
            # If current file exists and is a .cs file, we clear it.
            if (os.path.isfile(abs_path) and abs_path.endswith(".cs") and not abs_path.endswith("Ship.cs")):
                os.remove(abs_path)
        except Exception as e:
            # This exception will be raised if file is in use.
                print(e)

def classFile(fileName, className, shipName, shipRes, shipFaction):
    class_struct = [
        "using System;\n",
        "\n",
        "namespace SKPR.Ships\n",
        "{\n",
        "\tpublic class " + className + " " + ":" + " " + "Ship\n",
        "\t{\n",
        "\t\tstring Name = " + '"' + shipName + '"' + ";\n",
        "\t\tint[] Resources = " + shipRes + ";\n",
        "\t\tstring Faction = " + '"' + shipFaction + '"' + ";\n",
        "\t}\n",
        "}"
        ]
    with open(fileName, "w+") as file:
        file.writelines(class_struct);

# Change dir directly to avoid any confusion, and clear of .cs files
os.chdir("./Ships/")
clearFolder(os.getcwd())

# We open predefined ShipReadouts, strip all dashes, whitespaces and newlines,
# .. and create a new .cs file
with open("ShipReadouts.txt", "r") as file:
    for line in file:
        splitLine = line.split('.')
        className = splitLine[0].replace('-','').replace(' ','')
        shipName = splitLine[0]
        shipRes = splitLine[1]
        shipFaction = splitLine[2].replace('\n','')
        classFile(className + ".cs", className, shipName, shipRes, shipFaction)
