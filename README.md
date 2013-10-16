LaTeXTemplate
=============

A LaTeX template written in C#.  

Relies on having pdflatex installed somewhere.

It's not fast and it's not pretty, but it's a thing that I built and it was actually pretty fun.  As a bonus it even works (probably).
There's not a whole lot of C# tools for LaTeX and I feel like that's a shame.  

Anywho, the good bits:

The template syntax looks like:

	This is the start of the document
	%< Title >%
	this is the subtitle.
	beginLongTable{}
	\longTableLine{ID & Name & Quantity & Amount}
	%<for: Rows r >%
	%<  \longTableLine: {r.Name} {r.Quantity} {r.Amount}>%
	%<      if: r.HasSubItems>%
	\longTableLine{Sub Items & & Name & Price}
	%<          for: r.SubItems subItem>%
	%<              \longTableSubLine: {subItem.Name} {subItem.Price}>%
	%<          end:for>%
	%<      end:if>%
	%<end:for>%
	Grand Totals:
	%<Items>% - %<Total>%";
	
See what I did there?  Instead of "<% %>" I used "%< >%".  See it's clever because % is the symbol for a comment in so your templates are still valid TeX...  
Ok maybe it's dumb.  You can configure the symbols to whatever you want by modifying TemplateConsts.cs (hater).

Back to stuff you don't care about.  This was built as a mental exercise for myself.  I wanted to do something in pure TDD style and I've always gotten caught up on real world usage.  Yeah I can test drive a game of Tic Tack Toe, or some other simple program that fits into the confines of a book, but as soon as I went to work on something and was getting paid to do it... Well it never seemed to pan out and I always felt guilty about burning company time to write a bunch of fragile and overall terrible unit tests.  So this project was something that I wanted to do for myself as a moderately complex project that would require a lot of interface refactoring (one of my hangups).  Also something that I was doing on my own time and I wouldn't have to feel bad about spending hours on end navel gazing about my code.  I learned a lot about stuff and things and it was an experience that I'd wish on everyone.  

I also have a bunch of changes that are rolling around in the back of my head.  I started reading up on compilers because my vocabulary was insufficent to name some of my classes.  DataChain was for the longest time named ChainOfResponsibilityWotBindsTheVariables.  I still hate that class name.  

So yeah, the current version works, but it's kinda pokey and there's a lot of room for optimization and stuff.  It's been sitting on my hard drive unloved for about a month now and I figure putting it up on the internets may kick me back into messing with it again.

