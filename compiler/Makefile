.PHONY: diagram clean

clean:
	rm -rf {Ara,Ara.Ast,Ara.Ast.Tests,Ara.CodeGen,Ara.CodeGen.Tests,Ara.Parsing,Ara.Parsing.Tests}/{bin,obj}

diagram:
	dot -Tpdf bin/Debug/net6.0/ara.dot -o bin/Debug/net6.0/ara.pdf && open bin/Debug/net6.0/ara.pdf
