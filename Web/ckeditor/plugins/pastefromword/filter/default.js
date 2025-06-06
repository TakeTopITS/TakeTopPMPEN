﻿/*
 Copyright (c) 2003-2016, CKSource - Frederico Knabben. All rights reserved.
 For licensing, see LICENSE.md or http://ckeditor.com/license
*/
(function () {
    function p() { return !1 } function v(a, b) { var c, d = []; a.filterChildren(b); for (c = a.children.length - 1; 0 <= c; c--)d.unshift(a.children[c]), a.children[c].remove(); c = a.attributes; var e = a, n = !0, h; for (h in c) if (n) n = !1; else { var k = new CKEDITOR.htmlParser.element(a.name); k.attributes[h] = c[h]; e.add(k); e = k; delete c[h] } for (c = 0; c < d.length; c++)e.add(d[c]) } var f, g, l, m = CKEDITOR.tools, w = ["o:p", "xml", "script", "meta", "link"], t = {}, q = 0; CKEDITOR.plugins.pastefromword = {};
    CKEDITOR.cleanWord = function (a, b) {
        var c = Boolean(a.match(/mso-list:\s*l\d+\s+level\d+\s+lfo\d+/));
        a = a.replace(/<!\[/g, "\x3c!--[").replace(/\]>/g, "]--\x3e"); var d = CKEDITOR.htmlParser.fragment.fromHtml(a); l = new CKEDITOR.htmlParser.filter({
            root: function (a) { a.filterChildren(l); CKEDITOR.plugins.pastefromword.lists.cleanup(f.createLists(a)) }, elementNames: [[/^\?xml:namespace$/, ""], [/^v:shapetype/, ""], [new RegExp(w.join("|")), ""]], elements: {
                a: function (a) {
                    if (a.attributes.name) { if ("_GoBack" == a.attributes.name) { delete a.name; return } if (a.attributes.name.match(/^OLE_LINK\d+$/)) { delete a.name; return } } if (a.attributes.href &&
                        a.attributes.href.match(/#.+$/)) { var b = a.attributes.href.match(/#(.+)$/)[1]; t[b] = a } a.attributes.name && t[a.attributes.name] && (a = t[a.attributes.name], a.attributes.href = a.attributes.href.replace(/.*#(.*)$/, "#$1"))
                }, div: function (a) { g.createStyleStack(a, l, b) }, img: function (a) {
                    if (a.parent) { var b = a.parent.attributes; (b = b.style || b.STYLE) && b.match(/mso\-list:\s?Ignore/) && (a.attributes["cke-ignored"] = !0) } g.mapStyles(a, {
                        width: function (b) { g.setStyle(a, "width", b + "px") }, height: function (b) {
                            g.setStyle(a, "height",
                                b + "px")
                        }
                    }); a.attributes.src && a.attributes.src.match(/^file:\/\//) && a.attributes.alt && a.attributes.alt.match(/^https?:\/\//) && (a.attributes.src = a.attributes.alt)
                }, p: function (a) {
                    a.filterChildren(l); if (a.attributes.style && a.attributes.style.match(/display:\s*none/i)) return !1; if (f.thisIsAListItem(a)) f.convertToFakeListItem(a); else {
                        var c = a.getAscendant(function (a) { return "ul" == a.name || "ol" == a.name }), d = m.parseCssText(a.attributes.style); c && !c.attributes["cke-list-level"] && d["mso-list"] && d["mso-list"].match(/level/) &&
                            (c.attributes["cke-list-level"] = d["mso-list"].match(/level(\d+)/)[1])
                    } g.createStyleStack(a, l, b)
                }, pre: function (a) { f.thisIsAListItem(a) && f.convertToFakeListItem(a); g.createStyleStack(a, l, b) }, h1: function (a) { f.thisIsAListItem(a) && f.convertToFakeListItem(a); g.createStyleStack(a, l, b) }, font: function (a) { if (a.getHtml().match(/^\s*$/)) return (new CKEDITOR.htmlParser.text(" ")).insertAfter(a), !1; b && !0 === b.config.pasteFromWordRemoveFontStyles && a.attributes.size && delete a.attributes.size; v(a, l) }, ul: function (a) {
                    if (c) return "li" ==
                        a.parent.name && 0 === m.indexOf(a.parent.children, a) && g.setStyle(a.parent, "list-style-type", "none"), f.dissolveList(a), !1
                }, li: function (a) { c && (a.attributes.style = g.normalizedStyles(a, b), g.pushStylesLower(a)) }, ol: function (a) { if (c) return "li" == a.parent.name && 0 === m.indexOf(a.parent.children, a) && g.setStyle(a.parent, "list-style-type", "none"), f.dissolveList(a), !1 }, span: function (a) {
                    a.filterChildren(l); a.attributes.style = g.normalizedStyles(a, b); if (!a.attributes.style || a.attributes.style.match(/^mso\-bookmark:OLE_LINK\d+$/) ||
                        a.getHtml().match(/^(\s|&nbsp;)+$/)) { for (var c = a.children.length - 1; 0 <= c; c--)a.children[c].insertAfter(a); return !1 } g.createStyleStack(a, l, b)
                }, table: function (a) { a._tdBorders = {}; a.filterChildren(l); var b, c = 0, d; for (d in a._tdBorders) a._tdBorders[d] > c && (c = a._tdBorders[d], b = d); g.setStyle(a, "border", b) }, td: function (a) {
                    var b = a.getAscendant("table"), c = b._tdBorders, d = ["border", "border-top", "border-right", "border-bottom", "border-left"], b = m.parseCssText(b.attributes.style), e = b.background || b.BACKGROUND; e && g.setStyle(a,
                        "background", e, !0); (b = b["background-color"] || b["BACKGROUND-COLOR"]) && g.setStyle(a, "background-color", b, !0); var b = m.parseCssText(a.attributes.style), f; for (f in b) e = b[f], delete b[f], b[f.toLowerCase()] = e; for (f = 0; f < d.length; f++)b[d[f]] && (e = b[d[f]], c[e] = c[e] ? c[e] + 1 : 1); g.pushStylesLower(a, { background: !0 })
                }, "v:imagedata": p, "v:shape": function (a) {
                    var b = !1; a.parent.getFirst(function (c) { "img" == c.name && c.attributes && c.attributes["v:shapes"] == a.attributes.id && (b = !0) }); if (b) return !1; var c = ""; a.forEach(function (a) {
                    a.attributes &&
                        a.attributes.src && (c = a.attributes.src)
                    }, CKEDITOR.NODE_ELEMENT, !0); a.filterChildren(l); a.name = "img"; a.attributes.src = a.attributes.src || c; delete a.attributes.type
                }, style: function () { return !1 }
            }, attributes: { style: function (a, c) { return g.normalizedStyles(c, b) || !1 }, "class": function (a) { a = a.replace(/msonormal|msolistparagraph\w*/ig, ""); return "" === a ? !1 : a }, cellspacing: p, cellpadding: p, border: p, valign: p, "v:shapes": p, "o:spid": p }, comment: function (a) {
                a.match(/\[if.* supportFields.*\]/) && q++; "[endif]" == a && (q = 0 < q ? q -
                    1 : 0); return !1
            }, text: function (a) { return q ? "" : a.replace(/&nbsp;/g, " ") }
        }); var e = new CKEDITOR.htmlParser.basicWriter; l.applyTo(d); d.writeHtml(e); return e.getHtml()
    }; CKEDITOR.plugins.pastefromword.styles = {
        setStyle: function (a, b, c, d) { var e = m.parseCssText(a.attributes.style); d && e[b] || ("" === c ? delete e[b] : e[b] = c, a.attributes.style = CKEDITOR.tools.writeCssText(e)) }, mapStyles: function (a, b) {
            for (var c in b) if (a.attributes[c]) {
                if ("function" === typeof b[c]) b[c](a.attributes[c]); else g.setStyle(a, b[c], a.attributes[c]);
                delete a.attributes[c]
            }
        }, normalizedStyles: function (a, b) {
            var c = "background-color:transparent border-image:none color:windowtext direction:ltr mso- text-indent visibility:visible div:border:none".split(" "), d = "font-family font font-size color background-color line-height text-decoration".split(" "), e = function () { for (var a = [], b = 0; b < arguments.length; b++)arguments[b] && a.push(arguments[b]); return -1 !== m.indexOf(c, a.join(":")) }, n = b && !0 === b.config.pasteFromWordRemoveFontStyles, h = m.parseCssText(a.attributes.style);
            "cke:li" == a.name && h["TEXT-INDENT"] && h.MARGIN && (a.attributes["cke-indentation"] = f.getElementIndentation(a), h.MARGIN = h.MARGIN.replace(/(([\w\.]+ ){3,3})[\d\.]+(\w+$)/, "$10$3")); for (var k = m.objectKeys(h), r = 0; r < k.length; r++) { var g = k[r].toLowerCase(), u = h[k[r]], l = CKEDITOR.tools.indexOf; (n && -1 !== l(d, g.toLowerCase()) || e(null, g, u) || e(null, g.replace(/\-.*$/, "-")) || e(null, g) || e(a.name, g, u) || e(a.name, g.replace(/\-.*$/, "-")) || e(a.name, g) || e(u)) && delete h[k[r]] } return CKEDITOR.tools.writeCssText(h)
        }, createStyleStack: function (a,
            b, c) { var d = []; a.filterChildren(b); for (b = a.children.length - 1; 0 <= b; b--)d.unshift(a.children[b]), a.children[b].remove(); g.sortStyles(a); b = m.parseCssText(g.normalizedStyles(a, c)); c = a; var e = "span" === a.name, n; for (n in b) if (!n.match(/margin|text\-align|width|border|padding/i)) if (e) e = !1; else { var h = new CKEDITOR.htmlParser.element("span"); h.attributes.style = n + ":" + b[n]; c.add(h); c = h; delete b[n] } "{}" !== JSON.stringify(b) ? a.attributes.style = CKEDITOR.tools.writeCssText(b) : delete a.attributes.style; for (b = 0; b < d.length; b++)c.add(d[b]) },
        sortStyles: function (a) { for (var b = ["border", "border-bottom", "font-size", "background"], c = m.parseCssText(a.attributes.style), d = m.objectKeys(c), e = [], n = [], h = 0; h < d.length; h++)-1 !== m.indexOf(b, d[h].toLowerCase()) ? e.push(d[h]) : n.push(d[h]); e.sort(function (a, c) { var d = m.indexOf(b, a.toLowerCase()), e = m.indexOf(b, c.toLowerCase()); return d - e }); d = [].concat(e, n); e = {}; for (h = 0; h < d.length; h++)e[d[h]] = c[d[h]]; a.attributes.style = CKEDITOR.tools.writeCssText(e) }, pushStylesLower: function (a, b) {
            if (!a.attributes.style || 0 ===
                a.children.length) return !1; b = b || {}; var c = { "list-style-type": !0, width: !0, border: !0, "border-": !0 }, d = m.parseCssText(a.attributes.style), e; for (e in d) if (!(e.toLowerCase() in c || c[e.toLowerCase().replace(/\-.*$/, "-")] || e.toLowerCase() in b)) { for (var n = !1, h = 0; h < a.children.length; h++) { var k = a.children[h]; k.type === CKEDITOR.NODE_ELEMENT && (n = !0, g.setStyle(k, e, d[e])) } n && delete d[e] } a.attributes.style = CKEDITOR.tools.writeCssText(d); return !0
        }
    }; g = CKEDITOR.plugins.pastefromword.styles; CKEDITOR.plugins.pastefromword.lists =
        {
            thisIsAListItem: function (a) { return a.attributes.style && a.attributes.style.match(/mso\-list:\s?l\d/) && "li" !== a.parent.name || a.attributes["cke-dissolved"] || a.getHtml().match(/<!\-\-\[if !supportLists]\-\->/) || a.getHtml().match(/^( )*.*?[\.\)] ( ){2,666}/) ? !0 : !1 }, convertToFakeListItem: function (a) {
                this.getListItemInfo(a); if (!a.attributes["cke-dissolved"]) {
                    var b; a.forEach(function (a) { !b && "img" == a.name && a.attributes["cke-ignored"] && "*" == a.attributes.alt && (b = "·", a.remove()) }, CKEDITOR.NODE_ELEMENT); a.forEach(function (a) {
                    b ||
                        a.value.match(/^ /) || (b = a.value)
                    }, CKEDITOR.NODE_TEXT); if ("undefined" == typeof b) return; a.attributes["cke-symbol"] = b.replace(/ .*$/, ""); f.removeSymbolText(a)
                } if (a.attributes.style) { var c = m.parseCssText(a.attributes.style); c["margin-left"] && (delete c["margin-left"], a.attributes.style = CKEDITOR.tools.writeCssText(c)) } a.name = "cke:li"
            }, convertToRealListItems: function (a) { var b = []; a.forEach(function (a) { "cke:li" == a.name && (a.name = "li", b.push(a)) }, CKEDITOR.NODE_ELEMENT, !1); return b }, removeSymbolText: function (a) {
                var b,
                c = a.attributes["cke-symbol"]; a.forEach(function (d) { !b && d.value.match(c.replace(")", "\\)").replace("(", "")) && (d.value = d.value.replace(c, ""), d.parent.getHtml().match(/^(\s|&nbsp;)*$/) && (b = d.parent !== a ? d.parent : null)) }, CKEDITOR.NODE_TEXT); b && b.remove()
            }, setListSymbol: function (a, b, c) {
                c = c || 1; var d = m.parseCssText(a.attributes.style); if ("ol" == a.name) {
                    if (a.attributes.type || d["list-style-type"]) return; var e = { "[ivx]": "lower-roman", "[IVX]": "upper-roman", "[a-z]": "lower-alpha", "[A-Z]": "upper-alpha", "\\d": "decimal" },
                        n; for (n in e) if (f.getSubsectionSymbol(b).match(new RegExp(n))) { d["list-style-type"] = e[n]; break } a.attributes["cke-list-style-type"] = d["list-style-type"]
                } else e = { "·": "disc", o: "circle", "§": "square" }, !d["list-style-type"] && e[b] && (d["list-style-type"] = e[b]); f.setListSymbol.removeRedundancies(d, c); (a.attributes.style = CKEDITOR.tools.writeCssText(d)) || delete a.attributes.style
            }, setListStart: function (a) {
                for (var b = [], c = 0, d = 0; d < a.children.length; d++)b.push(a.children[d].attributes["cke-symbol"] || ""); b[0] || c++;
                switch (a.attributes["cke-list-style-type"]) { case "lower-roman": case "upper-roman": a.attributes.start = f.toArabic(f.getSubsectionSymbol(b[c])) - c; break; case "lower-alpha": case "upper-alpha": a.attributes.start = f.getSubsectionSymbol(b[c]).replace(/\W/g, "").toLowerCase().charCodeAt(0) - 96 - c; break; case "decimal": a.attributes.start = parseInt(f.getSubsectionSymbol(b[c]), 10) - c || 1 }"1" == a.attributes.start && delete a.attributes.start; delete a.attributes["cke-list-style-type"]
            }, numbering: {
                toNumber: function (a, b) {
                    function c(a) {
                        a =
                        a.toUpperCase(); for (var b = 1, c = 1; 0 < a.length; c *= 26)b += "ABCDEFGHIJKLMNOPQRSTUVWXYZ".indexOf(a.charAt(a.length - 1)) * c, a = a.substr(0, a.length - 1); return b
                    } function d(a) { var b = [[1E3, "M"], [900, "CM"], [500, "D"], [400, "CD"], [100, "C"], [90, "XC"], [50, "L"], [40, "XL"], [10, "X"], [9, "IX"], [5, "V"], [4, "IV"], [1, "I"]]; a = a.toUpperCase(); for (var c = b.length, d = 0, f = 0; f < c; ++f)for (var g = b[f], m = g[1].length; a.substr(0, m) == g[1]; a = a.substr(m))d += g[0]; return d } return "decimal" == b ? Number(a) : "upper-roman" == b || "lower-roman" == b ? d(a.toUpperCase()) :
                        "lower-alpha" == b || "upper-alpha" == b ? c(a) : 1
                }, getStyle: function (a) { a = a.slice(0, 1); var b = { i: "lower-roman", v: "lower-roman", x: "lower-roman", l: "lower-roman", m: "lower-roman", I: "upper-roman", V: "upper-roman", X: "upper-roman", L: "upper-roman", M: "upper-roman" }[a]; b || (b = "decimal", a.match(/[a-z]/) && (b = "lower-alpha"), a.match(/[A-Z]/) && (b = "upper-alpha")); return b }
            }, getSubsectionSymbol: function (a) { return (a.match(/([\da-zA-Z]+).?$/) || ["placeholder", 1])[1] }, setListDir: function (a) {
                var b = 0, c = 0; a.forEach(function (a) {
                "li" ==
                    a.name && ("rtl" == (a.attributes.dir || a.attributes.DIR || "").toLowerCase() ? c++ : b++)
                }, CKEDITOR.ELEMENT_NODE); c > b && (a.attributes.dir = "rtl")
            }, createList: function (a) { return (a.attributes["cke-symbol"].match(/([\da-np-zA-NP-Z]).?/) || [])[1] ? new CKEDITOR.htmlParser.element("ol") : new CKEDITOR.htmlParser.element("ul") }, createLists: function (a) {
                var b, c, d, e = f.convertToRealListItems(a); if (0 === e.length) return []; var n = f.groupLists(e); for (a = 0; a < n.length; a++) {
                    var h = n[a], k = h[0]; for (d = 0; d < h.length; d++)if (1 == h[d].attributes["cke-list-level"]) {
                        k =
                        h[d]; break
                    } var k = [f.createList(k)], g = k[0], m = [k[0]]; g.insertBefore(h[0]); for (d = 0; d < h.length; d++) {
                        b = h[d]; for (c = b.attributes["cke-list-level"]; c > k.length;) { var l = f.createList(b), p = g.children; 0 < p.length ? p[p.length - 1].add(l) : (p = new CKEDITOR.htmlParser.element("li", { style: "list-style-type:none" }), p.add(l), g.add(p)); k.push(l); m.push(l); g = l; c == k.length && f.setListSymbol(l, b.attributes["cke-symbol"], c) } for (; c < k.length;)k.pop(), g = k[k.length - 1], c == k.length && f.setListSymbol(g, b.attributes["cke-symbol"], c); b.remove();
                        g.add(b)
                    } k[0].children.length && (d = k[0].children[0].attributes["cke-symbol"], !d && 1 < k[0].children.length && (d = k[0].children[1].attributes["cke-symbol"]), d && f.setListSymbol(k[0], d)); for (d = 0; d < m.length; d++)f.setListStart(m[d]); for (d = 0; d < h.length; d++)this.determineListItemValue(h[d])
                } return e
            }, cleanup: function (a) { var b = ["cke-list-level", "cke-symbol", "cke-list-id", "cke-indentation", "cke-dissolved"], c, d; for (c = 0; c < a.length; c++)for (d = 0; d < b.length; d++)delete a[c].attributes[b[d]] }, determineListItemValue: function (a) {
                if ("ol" ===
                    a.parent.name) { var b = this.calculateValue(a), c = a.attributes["cke-symbol"].match(/[a-z0-9]+/gi), d; c && (c = c[c.length - 1], d = a.parent.attributes["cke-list-style-type"] || this.numbering.getStyle(c), c = this.numbering.toNumber(c, d), c !== b && (a.attributes.value = c)) }
            }, calculateValue: function (a) {
                if (!a.parent) return 1; var b = a.parent; a = a.getIndex(); var c = null, d, e, n; for (n = a; 0 <= n && null === c; n--)e = b.children[n], e.attributes && void 0 !== e.attributes.value && (d = n, c = parseInt(e.attributes.value, 10)); null === c && (c = void 0 !== b.attributes.start ?
                    parseInt(b.attributes.start, 10) : 1, d = 0); return c + (a - d)
            }, dissolveList: function (a) {
                function b(a) { return 50 <= a ? "l" + b(a - 50) : 40 <= a ? "xl" + b(a - 40) : 10 <= a ? "x" + b(a - 10) : 9 == a ? "ix" : 5 <= a ? "v" + b(a - 5) : 4 == a ? "iv" : 1 <= a ? "i" + b(a - 1) : "" } var c, d = [], e = []; a.forEach(function (a) {
                    if ("li" == a.name) {
                        var c = a.children[0]; if (c && c.name && c.attributes.style && c.attributes.style.match(/mso-list:/i)) {
                            g.pushStylesLower(a, { "list-style-type": !0, display: !0 }); var f = m.parseCssText(c.attributes.style, !0); g.setStyle(a, "mso-list", f["mso-list"], !0);
                            g.setStyle(c, "mso-list", ""); if (f.display || f.DISPLAY) f.display ? g.setStyle(a, "display", f.display, !0) : g.setStyle(a, "display", f.DISPLAY, !0)
                        } a.attributes.style && a.attributes.style.match(/mso-list:/i) && (a.name = "p", a.attributes["cke-dissolved"] = !0, d.push(a))
                    } if ("ul" == a.name || "ol" == a.name) {
                        for (c = 0; c < a.children.length; c++)if ("li" == a.children[c].name) {
                            var f = a.attributes.type, l = parseInt(a.attributes.start, 10) || 1; f || (f = m.parseCssText(a.attributes.style)["list-style-type"]); switch (f) {
                                case "disc": f = "·"; break; case "circle": f =
                                    "o"; break; case "square": f = "§"; break; case "1": case "decimal": f = l + c + "."; break; case "a": case "lower-alpha": f = String.fromCharCode(97 + l - 1 + c) + "."; break; case "A": case "upper-alpha": f = String.fromCharCode(65 + l - 1 + c) + "."; break; case "i": case "lower-roman": f = b(l + c) + "."; break; case "I": case "upper-roman": f = b(l + c).toUpperCase() + "."; break; default: f = "ul" == a.name ? "·" : l + c + "."
                            }a.children[c].attributes["cke-symbol"] = f
                        } e.push(a)
                    }
                }, CKEDITOR.NODE_ELEMENT, !1); for (c = d.length - 1; 0 <= c; c--)d[c].insertAfter(a); for (c = e.length - 1; 0 <=
                    c; c--)delete e[c].name
            }, groupLists: function (a) { var b, c, d = [[a[0]]], e = d[0]; c = a[0]; c.attributes["cke-indentation"] = c.attributes["cke-indentation"] || f.getElementIndentation(c); for (b = 1; b < a.length; b++) { c = a[b]; var g = a[b - 1]; c.attributes["cke-indentation"] = c.attributes["cke-indentation"] || f.getElementIndentation(c); c.previous !== g && (f.chopDiscontinuousLists(e, d), d.push(e = [])); e.push(c) } f.chopDiscontinuousLists(e, d); return d }, chopDiscontinuousLists: function (a, b) {
                for (var c = {}, d = [[]], e, g = 0; g < a.length; g++) {
                    var h =
                        c[a[g].attributes["cke-list-level"]], k = this.getListItemInfo(a[g]), l, p; h ? (p = h.type.match(/alpha/) && 7 == h.index ? "alpha" : p, p = "o" == a[g].attributes["cke-symbol"] && 14 == h.index ? "alpha" : p, l = f.getSymbolInfo(a[g].attributes["cke-symbol"], p), k = this.getListItemInfo(a[g]), (h.type != l.type || e && k.id != e.id && !this.isAListContinuation(a[g])) && d.push([])) : l = f.getSymbolInfo(a[g].attributes["cke-symbol"]); for (e = parseInt(a[g].attributes["cke-list-level"], 10) + 1; 20 > e; e++)c[e] && delete c[e]; c[a[g].attributes["cke-list-level"]] =
                            l; d[d.length - 1].push(a[g]); e = k
                } [].splice.apply(b, [].concat([m.indexOf(b, a), 1], d))
            }, isAListContinuation: function (a) { var b = a; do if ((b = b.previous) && b.type === CKEDITOR.NODE_ELEMENT) { if (void 0 === b.attributes["cke-list-level"]) break; if (b.attributes["cke-list-level"] === a.attributes["cke-list-level"]) return b.attributes["cke-list-id"] === a.attributes["cke-list-id"] } while (b); return !1 }, getElementIndentation: function (a) {
                a = m.parseCssText(a.attributes.style); if (a.margin || a.MARGIN) {
                a.margin = a.margin || a.MARGIN; var b =
                    { styles: { margin: a.margin } }; CKEDITOR.filter.transformationsTools.splitMarginShorthand(b); a["margin-left"] = b.styles["margin-left"]
                } return parseInt(m.convertToPx(a["margin-left"] || "0px"), 10)
            }, toArabic: function (a) {
                return a.match(/[ivxl]/i) ? a.match(/^l/i) ? 50 + f.toArabic(a.slice(1)) : a.match(/^lx/i) ? 40 + f.toArabic(a.slice(1)) : a.match(/^x/i) ? 10 + f.toArabic(a.slice(1)) : a.match(/^ix/i) ? 9 + f.toArabic(a.slice(2)) : a.match(/^v/i) ? 5 + f.toArabic(a.slice(1)) : a.match(/^iv/i) ? 4 + f.toArabic(a.slice(2)) : a.match(/^i/i) ? 1 + f.toArabic(a.slice(1)) :
                    f.toArabic(a.slice(1)) : 0
            }, getSymbolInfo: function (a, b) {
                var c = a.toUpperCase() == a ? "upper-" : "lower-", d = { "·": ["disc", -1], o: ["circle", -2], "§": ["square", -3] }; if (a in d || b && b.match(/(disc|circle|square)/)) return { index: d[a][1], type: d[a][0] }; if (a.match(/\d/)) return { index: a ? parseInt(f.getSubsectionSymbol(a), 10) : 0, type: "decimal" }; a = a.replace(/\W/g, "").toLowerCase(); return !b && a.match(/[ivxl]+/i) || b && "alpha" != b || "roman" == b ? { index: f.toArabic(a), type: c + "roman" } : a.match(/[a-z]/i) ? {
                    index: a.charCodeAt(0) - 97, type: c +
                        "alpha"
                } : { index: -1, type: "disc" }
            }, getListItemInfo: function (a) { if (void 0 !== a.attributes["cke-list-id"]) return { id: a.attributes["cke-list-id"], level: a.attributes["cke-list-level"] }; var b = m.parseCssText(a.attributes.style)["mso-list"], c = { id: "0", level: "1" }; b && (b += " ", c.level = b.match(/level(.+?)\s+/)[1], c.id = b.match(/l(\d+?)\s+/)[1]); a.attributes["cke-list-level"] = void 0 !== a.attributes["cke-list-level"] ? a.attributes["cke-list-level"] : c.level; a.attributes["cke-list-id"] = c.id; return c }
        }; f = CKEDITOR.plugins.pastefromword.lists;
    f.setListSymbol.removeRedundancies = function (a, b) { (1 === b && "disc" === a["list-style-type"] || "decimal" === a["list-style-type"]) && delete a["list-style-type"] }; CKEDITOR.plugins.pastefromword.createAttributeStack = v
})();