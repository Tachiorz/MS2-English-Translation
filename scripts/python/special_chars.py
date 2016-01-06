import codecs
from lxml import etree as et

xml_file = ""

parser = et.XMLParser(ns_clean=False, recover=True, remove_comments=False, remove_blank_text=False, encoding='utf-8')
tree = et.parse(xml_file, parser=parser)

print("Replacing")
for i in tree.iter():
    for k, v in i.attrib.iteritems():
        v = v.replace(u'\u2019', '\'')
        i.attrib[k] = v

with codecs.open(xml_file, 'w', 'utf-8') as f:
    xml_string = et.tostring(tree, encoding='UTF-8', xml_declaration=True)
    f.write(xml_string.replace('&#10;', '\\n').replace('&#8217;', '\'').decode('utf-8'))
print("finished")
