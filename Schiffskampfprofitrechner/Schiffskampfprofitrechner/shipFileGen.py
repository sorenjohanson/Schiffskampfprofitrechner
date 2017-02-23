# Ship File Generator
# This tool will automatically read through a provided textfile and, after stripping whitespaces and dashes, create a .cs Class file using that name.

import os, shutil

def clearFolder(folder):
    for fil in os.listdir(folder): 
        abs_path = os.path.join(folder, fil)
        try:
            # If current file exists and is a .cs file, we clear it.
            if (os.path.isfile(abs_path) and abs_path.endswith(".cs")):
                os.remove(abs_path)
        except Exception as e:
            # This exception will be raised if file is in use.
                print(e)

def classFile(fileName, name):
    class_struct = [
        "using System;\n",
        "\n",
        "namespace SKPR.Ships\n",
        "{\n",
        "\tpublic class " + name + "\n",
        "\t{\n",
        "\t\tstring Name { get; set; }\n",
        "\t\tint[] Resources { get; set; }\n",
        "\t\tstring Faction { get; set; }\n",
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
        short = line.replace('-','').replace(' ','').strip()
        classFile(short + ".cs", short)
