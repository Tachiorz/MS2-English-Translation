from xml.etree import ElementTree as et

xml_file = ""
tree = et.parse(xml_file)
key_iterator = tree.findall('key')

print("Replacing")
for i in key_iterator:
	name = i.get('name')
	if name: # Need to check we have a name as some item ids don't have names - wtf? 
		name = name.encode("ascii", 'xmlcharrefreplace').replace('&#8217;','\'')
		i.set('name', name)

tree.write(xml_file)
print("finished")