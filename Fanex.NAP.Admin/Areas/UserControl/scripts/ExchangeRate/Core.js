if (typeof (Fanex) == 'undefined') { // Require Fanex namespace
    Fanex = {};
    Fanex.CloneObject = function (obj) {
        var newObj = (obj instanceof Array) ? [] : {};
        for (i in obj) {
            if (i == 'clone') continue;
            if (obj[i] && typeof obj[i] == "object") {
                newObj[i] = Fanex.CloneObject(obj[i]);
            } else {
                newObj[i] = obj[i];
            }
        }
        return newObj;
    };
    String.Empty = ""; // Extend static string
    String.prototype.Format = function (args) {
        var formatted = this;
        for (var i = 0; i < arguments.length; i++) {
            var regexp = new RegExp('\\{' + i + '\\}', 'gi');
            formatted = formatted.replace(regexp, arguments[i]);
        }
        return formatted;
    };
    String.prototype.FormatNumber = function (number) { // Format a number to 1,234,567.89
        number += String.Empty;
        x = number.split('.');
        x1 = x[0];
        x2 = x.length > 1 ? '.' + x[1] : String.Empty;
        var rgx = /(\d+)(\d{3})/;
        while (rgx.test(x1)) {
            x1 = x1.replace(rgx, '$1' + ',' + '$2');
        }
        return this + x1 + x2;
    };
    String.prototype.FormatNumber5Decimal = function (number) { // Format a number to 1,234,567.89
        number = (Math.round(number * 100000) / 100000).toString();
        x = number.split('.');
        x1 = x[0];
        x2 = x.length > 1 ? '.' + x[1] : String.Empty;
        var rgx = /(\d+)(\d{3})/;
        while (rgx.test(x1)) {
            x1 = x1.replace(rgx, '$1' + ',' + '$2');
        }
        return this + x1 + x2;
    };
    String.prototype.escapeHtml = function () { // Replace all ampersands with &amp; and all <’s and >’s with &lt; and &gt;, respectively:
        var result = String.Empty;
        for (var i = 0; i < this.length; i++) {
            if (this.charAt(i) == "&" && this.length - i - 1 >= 4 && this.substr(i, 4) != "&amp;") {
                result = result + "&amp;";
            } else if (this.charAt(i) == "<") {
                result = result + "&lt;";
            } else if (this.charAt(i) == ">") {
                result = result + "&gt;";
            } else {
                result = result + this.charAt(i);
            }
        }
        return result;
    };
    (function () { // Parse float with string has commas inside parseFloat(string, true)
        var proxied = window.parseFloat;
        window.parseFloat = function () {
            if (arguments[1] === true && typeof (arguments[0]) == 'string') {
                arguments[0] = arguments[0].replace(/,/g, String.Empty); // Replace all occurrences of comma to empty
            }
            return proxied.apply(this, arguments);
        };
    })();
    Fanex.FormatNumberFloat = function (number) { // Format a number to 12,345
        if (number == String.Empty) return 0;
        var sign;
        if (isNaN(number)) {
            number = String.Empty;
        }
        //sign = (number == (number = Math.abs(number)));
        //number = Math.floor(number * 100 + 0.50000000001);
        //number = Math.floor(number / 100).toString();
        for (var i = 0; i < Math.floor((number.length - (1 + i)) / 3); i++) {
            number = number.substring(0, number.length - (4 * i + 3)) + ',' + number.substring(number.length - (4 * i + 3));
        }
        //return (((sign) ? String.Empty : '-') + number);
        return number;
    };
    // Format input number on event e, using: onkeyup=Fanex.FormatNumber(event)
    Fanex.FormatNumber = function (event, isFloat, length) {
        var evt = event ? event : window.event; // Fix for IE7

        if (evt.keyCode === 9) return true; // Ignore TAB key

        var obj = evt.currentTarget ? evt.currentTarget : evt.srcElement; // Fix for IE7
        var value = obj.value;
        var num;
        var dotPosition = 0;
        var afterDot;
        var caretPosition = Fanex.GetCaretPosition(obj);

        num = value.toString().replace(/\$|\,/g, String.Empty);
        if (num.length > length) {
            num = num.substring(0, length);
        }

        dotPosition = num.indexOf(".");
        if ((typeof (isFloat) != 'undefined') && isFloat && dotPosition > 0) {
            afterDot = num.substring(dotPosition + 1, num.length).replace(/[^0-9]/g, String.Empty);
            num = num.substring(0, dotPosition);
        }
        num = Fanex.FormatNumberFloat(num);
        if ((typeof (isFloat) != 'undefined') && isFloat && dotPosition > 0) {
            eval(obj).value = num + "." + afterDot;
            num = obj.value;
        } else {
            eval(obj).value = num;
        }

        if (num.length > value.length)
            caretPosition = caretPosition + 1;
        Fanex.SetCaretPosition(obj, caretPosition);
        return true;
    };

    Fanex.FormatNumberAndDot = function (event, isFloat) {
        var evt = event ? event : window.event; // Fix for IE7
        if (evt.keyCode === 9 || evt.keyCode === 110 || evt.keyCode === 190) return true; // Ignore TAB key

        var obj = evt.currentTarget ? evt.currentTarget : evt.srcElement; // Fix for IE7
        var value = obj.value;
        var num;
        var dotPosition = 0;
        var afterDot;
        var caretPosition = Fanex.GetCaretPosition(obj);

        num = value.toString().replace(/\$|\,/g, String.Empty);
        if (num.length > 15) {
            num = num.substring(0, 15);
        }
        
        if (evt.keyCode != 96) {
            num = (Math.round(num * 100000) / 100000).toString();
        }
        else {
            if (value.indexOf('.') != -1) {
                var arr = value.split('.');
                if (arr[1].length > 5) {
                    //num = num.substring(0, num.length -1);
                    num = (Math.round(num * 100000) / 100000).toString();
                }
            }
        }
        dotPosition = num.indexOf(".");
        if ((typeof (isFloat) != 'undefined') && isFloat && dotPosition > 0) {
            afterDot = num.substring(dotPosition + 1, num.length).replace(/[^0-9]/g, String.Empty);
            num = num.substring(0, dotPosition);
        }
        num = Fanex.FormatNumberFloat(num);
        if ((typeof (isFloat) != 'undefined') && isFloat && dotPosition > 0) {
            eval(obj).value = num + "." + afterDot;
            num = obj.value;
        } else {
            eval(obj).value = num;
        }

        if (num.length > value.length)
            caretPosition = caretPosition + 1;
        Fanex.SetCaretPosition(obj, caretPosition);
        return true;
    };

    // Arguments: number to round, number of decimal places
    Fanex.RoundNumber = function (number, decimals) {
        var newnumber = new Number(number + '').toFixed(parseInt(decimals));
        return parseFloat(newnumber); // Output the result to the form field (change for your purposes)
    };

    Fanex.HandleKeyDownInputNumber = function (e) {
        e = e || event;
        var charCode = ('charCode' in e ? e.charCode : e.keyCode);

        // allow special key presses
        if (charCode == 0 || e.altKey || e.ctrlKey || e.metaKey) return;

        // allow digits
        if (charCode >= 48 && charCode <= 57) return;

        // prevent the default action
        if (e.preventDefault) {
            e.preventDefault();
        } else {
            window.event.returnValue = false;
        }
    }

    Fanex.HandleKeyDownInputNumberAndDot = function (e) {
        e = e || event;
        var charCode = ('charCode' in e ? e.charCode : e.keyCode);
        var value = $(this).val();

        if (value.indexOf('.') == -1 && charCode == 46) {
            return;
        }

        // allow special key presses
        if (charCode == 0 || e.altKey || e.ctrlKey || e.metaKey) return;

        // allow digits
        if (charCode >= 48 && charCode <= 57) return;

        // prevent the default action
        if (e.preventDefault) {
            e.preventDefault();
        } else {
            window.event.returnValue = false;
        }


    }

    Fanex.Round = function (value) { //Format a number from 0.0199999 to 0.02
        return Math.round(value * 10000) / 10000;
    };

    // Returns the caret (cursor) position of the specified text field.
    Fanex.GetCaretPosition = function (oField) {
        var iCaretPos = 0;

        // IE Support
        if (document.selection) {
            oField.focus();
            // To get cursor position, get empty selection range
            var oSel = document.selection.createRange();
            // Move selection start to 0 position
            oSel.moveStart('character', -oField.value.length);
            iCaretPos = oSel.text.length;
        }
        // Firefox support
        else if (oField.selectionStart || oField.selectionStart == '0')
            iCaretPos = oField.selectionStart;

        // Return results
        return (iCaretPos);
    };

    // Sets the caret (cursor) position of the specified text field.
    Fanex.SetCaretPosition = function (oField, iCaretPos) {
        // IE Support
        if (document.selection) {
            oField.focus();

            // Create empty selection range
            var oSel = document.selection.createRange();

            // Move selection start and end to desired position
            oSel.moveStart('character', iCaretPos);
            oSel.moveEnd('character', iCaretPos - oField.value.length);
            oSel.select();
        }
        // Firefox support
        else if (oField.selectionStart || oField.selectionStart == '0') {
            oField.selectionStart = iCaretPos;
            oField.selectionEnd = iCaretPos;
            oField.focus();
        }
    }

    Fanex.ValidateMessage = function (elementInput, errorMessage, xpos, pointerx) {
        var isXPos = (typeof (xpos) != "undefined" && xpos != null);
        var isPointerXPos = (typeof (pointerx) != "undefined" && pointerx != null);
        if (isXPos && pointerx) {
            Validators.show(elementInput, errorMessage, xpos, pointerx, -45, 38, 18, 5000, false);
        }
        else {
            Validators.show(elementInput, errorMessage, -150, 210, -45, 38, 18, 5000, false);
        }
        Validators.animate(elementInput);
    };
};

// Validators library
var Validators = {
    add: function (elementInput, expr, message) {
        // Bind onchange event of input
        // Validate with expr
        // Show message and animate background color
    },
    show: function (elementInput, message, xPos, pointerXPos, yPos, pointerYPos, height, timeout, isDown) {
        // <span class="hint"><span>This is the message.<span><span class="hint-pointer">&nbsp;</span></span>
        var hint = document.getElementById('fHint');
        var hintPointer = null;
        if (hint == null) {
            var hint = document.createElement("SPAN");
            hint.className = 'hint';
            var hintMsg = document.createElement("SPAN");
            hint.appendChild(hintMsg);
            hintPointer = document.createElement("SPAN");
            //            hintPointer.className = 'hint-pointer';
            //            if (isDown) {
            //                hintPointer.className = 'hint-pointer-down';
            //            }

            hint.appendChild(hintPointer);
            hint.id = 'fHint';
            hint.style.top = '10px';
            hint.style.left = '10px';
            hint.onclick = function () { // Hide onclick
                hint.style.display = 'none';
            };
            document.body.appendChild(hint);
        };
        hintPointer = hint.lastChild;
        hintPointer.className = 'hint-pointer';
        if (isDown) {
            hintPointer.className = 'hint-pointer-down';
        }
        hint.firstChild.innerHTML = message;
        if (this.timeOutHideTooltip) {
            clearTimeout(this.timeOutHideTooltip);
            this.timeOutHideTooltip = null;
        };
        if (typeof (timeout) == 'undefined') {
            timeout = 8000;
        }
        this.timeOutHideTooltip = setTimeout(function () {
            if (hint == null) {
                return;
            }
            hint.style.display = 'none';
        }, timeout); // Auto hide in 10s
        if (elementInput != null) {
            if (typeof xPos == 'undefined') {
                xPos = 0;
            }
            if (typeof pointerXPos == 'undefined') {
                pointerXPos = 10;
            }
            if (typeof yPos == 'undefined') {
                yPos = -45;
            }
            if (typeof (pointerYPos) == 'undefined') {
                pointerYPos = 38;
            }
            if (typeof (height) == 'undefined') {
                height = 18;
            }
            hint.style.height = height + 'px';
            hintPointer.style.left = (pointerXPos + 'px');
            hintPointer.style.top = (pointerYPos + 'px');
            var pos = Validators.utils.findPos(elementInput);
            hint.style.left = (pos[0] + xPos) + 'px';
            hint.style.top = (pos[1] + yPos) + 'px';
            //            elementInput.onchange = function () {
            //                hint.style.display = 'none';
            //            };
            elementInput.focus();
        };
        hint.style.display = 'block';
    },
    hide: function () {
        var hint = document.getElementById('fHint');
        if (hint == null) {
            return;
        }
        hint.style.display = 'none';
    },
    animate: function (input) {
        var previousColor = input.style.color;
        var previousBackground = input.style.backgroundColor;
        var c = 6;
        if (this.intervalAnimate) {
            clearInterval(this.intervalAnimate);
            this.intervalAnimate = null;
        }
        var int = this.intervalAnimate = setInterval(function () {
            c--;
            if (input.style.backgroundColor == previousBackground) {
                input.style.backgroundColor = '#ff7700';
                input.style.color = '#ffffff';
            } else {
                input.style.backgroundColor = previousBackground;
                input.style.color = previousColor;
            }
            if (c == 0) {
                input.style.backgroundColor = '';
                input.style.color = '';
                clearInterval(int);
            }
        }, 300);
    },
    utils: {
        // findPos() by quirksmode.org
        // Finds the absolute position of an element on a page
        findPos: function (obj) {
            var curleft = curtop = 0;
            if (obj.offsetParent) {
                do {
                    curleft += obj.offsetLeft;
                    curtop += obj.offsetTop;
                } while (obj = obj.offsetParent);
            }
            return [curleft, curtop];
        },
        // getPageScroll() by quirksmode.org
        // Finds the scroll position of a page
        getPageScroll: function () {
            var xScroll, yScroll;
            if (self.pageYOffset) {
                yScroll = self.pageYOffset;
                xScroll = self.pageXOffset;
            } else if (document.documentElement && document.documentElement.scrollTop) {
                yScroll = document.documentElement.scrollTop;
                xScroll = document.documentElement.scrollLeft;
            } else if (document.body) { // all other Explorers
                yScroll = document.body.scrollTop;
                xScroll = document.body.scrollLeft;
            }
            return [xScroll, yScroll]
        },
        // Finds the position of an element relative to the viewport.
        findPosRelativeToViewport: function (obj) {
            var objPos = this.findPos(obj)
            var scroll = this.getPageScroll()
            return [objPos[0] - scroll[0], objPos[1] - scroll[1]]
        }
    }
};

// Modal Popup
var Popups = {
    Confirm: function (max, title) {
        ModalPopups.Confirm("idConfirm",
                title,
                "<div style='padding: 10px;'>" + Language.WANT_TO_UPDATE_ALL_MEMBER_POSITION_TAKING + "<select id='selectedMemberPositionTaking' name='selectedMemberPositionTaking'/></div>",
                {
                    yesButtonText: Language.YES,
                    noButtonText: Language.NO,
                    onYes: "ConfirmYes()",
                    onNo: "ConfirmNo()",
                    width: 400
                }

            );
        Fanex.CustSetting.PT_SetSelect(document.getElementById("selectedMemberPositionTaking"), 0, max, Fanex.CustSetting.PT_STEP, max);
    },

    Close: function () {
        ModalPopups.Close("idConfirm");
    }
};

// Modal popup library is compressed
var ModalPopupsDefaults = { shadow: true, shadowSize: 5, shadowColor: "#999999", backgroundColor: "#FAFBFC", borderColor: "#999999", titleBackColor: "#3C3C3C", titleFontColor: "#ffffff", popupBackColor: "#ECECEC", popupFontColor: "black", footerBackColor: "#ECECEC", footerFontColor: "#15428B", okButtonText: "OK", yesButtonText: "Yes", noButtonText: "No", cancelButtonText: "Cancel", fontFamily: "Tahoma,Arial,helvetica,sans-serif", fontSize: "8pt" }; var ModalPopups = { Init: function () { }, SetDefaults: function (a) { a = a || {}; ModalPopupsDefaults.shadow = a.shadow != null ? a.shadow : ModalPopupsDefaults.shadow; ModalPopupsDefaults.shadowSize = a.shadowSize != null ? a.shadowSize : ModalPopupsDefaults.shadowSize; ModalPopupsDefaults.shadowColor = a.shadowColor != null ? a.shadowColor : ModalPopupsDefaults.shadowColor; ModalPopupsDefaults.backgroundColor = a.backgroundColor != null ? a.backgroundColor : ModalPopupsDefaults.backgroundColor; ModalPopupsDefaults.borderColor = a.borderColor != null ? a.borderColor : ModalPopupsDefaults.borderColor; ModalPopupsDefaults.okButtonText = a.okButtonText != null ? a.okButtonText : ModalPopupsDefaults.okButtonText; ModalPopupsDefaults.yesButtonText = a.yesButtonText != null ? a.yesButtonText : ModalPopupsDefaults.yesButtonText; ModalPopupsDefaults.noButtonText = a.noButtonText != null ? a.noButtonText : ModalPopupsDefaults.noButtonText; ModalPopupsDefaults.cancelButtonText = a.cancelButtonText != null ? a.cancelButtonText : ModalPopupsDefaults.cancelButtonText; ModalPopupsDefaults.titleBackColor = a.titleBackColor != null ? a.titleBackColor : ModalPopupsDefaults.titleBackColor; ModalPopupsDefaults.titleFontColor = a.titleFontColor != null ? a.titleFontColor : ModalPopupsDefaults.titleFontColor; ModalPopupsDefaults.popupBackColor = a.popupBackColor != null ? a.popupBackColor : ModalPopupsDefaults.popupBackColor; ModalPopupsDefaults.popupFontColor = a.popupFontColor != null ? a.popupFontColor : ModalPopupsDefaults.popupFontColor; ModalPopupsDefaults.footerBackColor = a.footerBackColor != null ? a.footerBackColor : ModalPopupsDefaults.footerBackColor; ModalPopupsDefaults.footerFontColor = a.footerFontColor != null ? a.footerFontColor : ModalPopupsDefaults.footerFontColor; ModalPopupsDefaults.fontFamily = a.fontFamily != null ? a.fontFamily : ModalPopupsDefaults.fontFamily; ModalPopupsDefaults.fontSize = a.fontSize != null ? a.fontSize : ModalPopupsDefaults.fontSize }, Alert: function (f, d, b, a) { a = a || {}; if (!d) { d = "Alert" } a.buttons = "ok"; a.okButtonText = a.okButtonText != null ? a.okButtonText : ModalPopupsDefaults.okButtonText; var e = ModalPopups._createAllLayers(f, d, b, a); var c = e[4]; c.innerHTML = b; ModalPopups._styleAllLayers(f, a, e) }, Confirm: function (f, d, a, b) { b = b || {}; if (!d) { d = "Confirm" } b.buttons = "yes,no"; b.yesButtonText = b.yesButtonText != null ? b.yesButtonText : ModalPopupsDefaults.yesButtonText; b.noButtonText = b.noButtonText != null ? b.noButtonText : ModalPopupsDefaults.noButtonText; var e = ModalPopups._createAllLayers(f, d, a, b); var c = e[4]; c.innerHTML = a; ModalPopups._styleAllLayers(f, b, e) }, YesNoCancel: function (f, d, a, b) { b = b || {}; if (!d) { d = "YesNoCancel" } b.buttons = "yes,no,cancel"; b.yesButtonText = b.yesButtonText != null ? b.yesButtonText : ModalPopupsDefaults.yesButtonText; b.noButtonText = b.noButtonText != null ? b.noButtonText : ModalPopupsDefaults.noButtonText; b.cancelButtonText = b.cancelButtonText != null ? b.cancelButtonText : ModalPopupsDefaults.cancelButtonText; var e = ModalPopups._createAllLayers(f, d, a, b); var c = e[4]; c.innerHTML = a; ModalPopups._styleAllLayers(f, b, e) }, Prompt: function (h, f, a, d) { d = d || {}; if (!f) { f = "Prompt" } d.buttons = "ok,cancel"; d.okButtonText = d.okButtonText != null ? d.okButtonText : "OK"; d.cancelButtonText = d.cancelButtonText != null ? d.cancelButtonText : "Cancel"; var g = ModalPopups._createAllLayers(h, f, a, d); var e = g[4]; var c = ""; if (d.width != null) { c = "width:95%;" } var b = a + "<br/>"; b += "<input type=text id='" + h + "_promptInput' value='' style='border: solid 1px #859DBE; " + c + "'>"; e.innerHTML = b; ModalPopups._styleAllLayers(h, d, g); ModalPopupsSupport.findControl(h + "_promptInput").focus() }, GetPromptInput: function (b) { var a = ModalPopupsSupport.findControl(b + "_promptInput"); return a }, GetPromptResult: function (b) { var a = ModalPopupsSupport.findControl(b + "_promptInput"); return a }, GetCustomControl: function (a) { return ModalPopupsSupport.findControl(a) }, Indicator: function (f, d, b, a) { a = a || {}; if (!d) { d = "Indicator" } if (a.buttons == null) { a.buttons = "" } var e = ModalPopups._createAllLayers(f, d, b, a); var c = e[4]; c.innerHTML = b; ModalPopups._styleAllLayers(f, a, e) }, Custom: function (f, d, b, a) { a = a || {}; if (!d) { d = "Custom" } if (a.buttons == null) { alert("buttons is a required parameter. ie: buttons: 'yes,no' or buttons: 'ok'.\nPossible buttons are yes, no, ok, cancel"); return } var e = ModalPopups._createAllLayers(f, d, b, a); var c = e[4]; c.innerHTML = b; ModalPopups._styleAllLayers(f, a, e) }, Close: function (a) { window.onresize = null; window.onscroll = null; document.body.removeChild(ModalPopupsSupport.findControl(a + "_background")); document.body.removeChild(ModalPopupsSupport.findControl(a + "_popup")); document.body.removeChild(ModalPopupsSupport.findControl(a + "_shadow")) }, Cancel: function (a) { ModalPopups.Close(a) }, _zIndex: 10000, _createAllLayers: function (l, t, h, e) { var i = ModalPopupsSupport.makeLayer(l + "_background", true, null); var r = ModalPopupsSupport.makeLayer(l + "_popup", true, null); var b = ModalPopupsSupport.makeLayer(l + "_shadow", true, null); var n = ModalPopupsSupport.makeLayer(l + "_popupTitle", true, r); var d = ModalPopupsSupport.makeLayer(l + "_popupBody", true, r); var m = ModalPopupsSupport.makeLayer(l + "_popupFooter", true, r); var j = e.okButtonText != null ? e.okButtonText : ModalPopupsDefaults.okButtonText; var p = e.yesButtonText != null ? e.yesButtonText : ModalPopupsDefaults.yesButtonText; var c = e.noButtonText != null ? e.noButtonText : ModalPopupsDefaults.noButtonText; var o = e.cancelButtonText != null ? e.cancelButtonText : ModalPopupsDefaults.cancelButtonText; var s = e.onOk != null ? e.onOk : 'ModalPopups.Close("' + l + '");'; var a = e.onYes != null ? e.onYes : 'ModalPopups.Close("' + l + '");'; var k = e.onNo != null ? e.onNo : 'ModalPopups.Close("' + l + '");'; var g = e.onCancel != null ? e.onCancel : 'ModalPopups.Close("' + l + '");'; n.innerHTML = "<div style='float: left'><b>" + t + "</b></div><div class='closePopup' onclick='javascript:Popups.Close();'></div>"; m.innerHTML = ""; e.fontFamily = e.fontFamily != null ? e.fontFamily : ModalPopupsDefaults.fontFamily; var q = e.buttons.split(","); for (x in q) { if (q[x] == "ok") { m.innerHTML += "<input name='" + l + "_okButton' id='" + l + "_okButton' type=button value='" + j + "' style='font-family:Verdana,Arial; font-size:8pt; border: solid 1px #859DBE; background-color: white; width:75px; height:20px; margin-right: 5px; margin-left: 5px;' onclick='" + s + "'/>" } if (q[x] == "yes") { m.innerHTML += "<input name='" + l + "_yesButton' id='" + l + "_yesButton' type=button value='" + p + "' style='font-family:Verdana,Arial; font-size:8pt; border: solid 1px #859DBE; background-color: white; width:75px; height:20px; margin-right: 5px; margin-left: 5px; cursor: pointer;' onclick='" + a + "'/>" } if (q[x] == "no") { m.innerHTML += "<input name='" + l + "_noButton' id='" + l + "_noButton' type=button value='" + c + "' style='font-family:Verdana,Arial; font-size:8pt; border: solid 1px #859DBE; background-color: white; width:75px; height:20px; margin-right: 5px; margin-left: 5px; cursor: pointer;' onclick='" + k + "'/>" } if (q[x] == "cancel") { m.innerHTML += "<input name='" + l + "_cancelButton' id='" + l + "_cancelButton' type=button value='" + o + "' style='font-family:Verdana,Arial; font-size:8pt; border: solid 1px #859DBE; background-color: white; width:75px; height:20px; margin-right: 5px; margin-left: 5px;' onclick='" + g + "'/>" } } var f = new Array(i, r, b, n, d, m); if (e.autoClose != null) { setTimeout('ModalPopups.Close("' + l + '");', e.autoClose) } return f }, _styleAllLayers: function (n, e, h) { var i = h; var k = i[0]; var t = i[1]; var a = i[2]; var r = i[3]; var d = i[4]; var p = i[5]; ModalPopups._zIndex += 3; var m = ModalPopups._zIndex; e.borderColor = e.borderColor != null ? e.borderColor : ModalPopupsDefaults.borderColor; var w = "display:inline; position:absolute; z-index: " + (m) + "; left:0px; top:0px; width:100%; height:100%; filter:alpha(opacity=70); opacity:0.7;"; if (ModalPopupsSupport.isOlderIE()) { var s = ModalPopupsSupport.getViewportDimensions(); w = "display:inline; position:absolute; z-index: 10; left:0px; top:0px; width:" + s.width + "px; height:" + s.height + "px; filter:alpha(opacity=70); opacity:0.7; overflow:hidden;" } var f = "display:inline; position:absolute; z-index: " + (m + 1) + ";"; var u = "display:inline; position:absolute; z-index: " + (m + 2) + "; background-color:white; color:black; border:solid 1px " + e.borderColor + "; padding:1px;"; e.backgroundColor = e.backgroundColor != null ? e.backgroundColor : ModalPopupsDefaults.backgroundColor; w += " background-color:" + e.backgroundColor + ";"; e.fontFamily = e.fontFamily != null ? e.fontFamily : ModalPopupsDefaults.fontFamily; e.fontSize = e.fontSize != null ? e.fontSize : ModalPopupsDefaults.fontSize; var c = "position: absolute; font-family:" + e.fontFamily + "; font-size:" + e.fontSize + "; padding: 5px; text-align:left;"; var l = "position: absolute; font-family:" + e.fontFamily + "; font-size:" + e.fontSize + "; padding: 5px; text-align:left;"; var v = "position: absolute; font-family:" + e.fontFamily + "; font-size:" + e.fontSize + "; padding: 5px; text-align:center;"; if (ModalPopupsSupport.isIE) { r.style.cssText = c; d.style.cssText = l; p.style.cssText = v } else { r.setAttribute("style", c); d.setAttribute("style", l); p.setAttribute("style", v) } e.titleBackColor = e.titleBackColor != null ? e.titleBackColor : ModalPopupsDefaults.titleBackColor; e.titleFontColor = e.titleFontColor != null ? e.titleFontColor : ModalPopupsDefaults.titleFontColor; e.popupBackColor = e.popupBackColor != null ? e.popupBackColor : ModalPopupsDefaults.popupBackColor; e.popupFontColor = e.popupFontColor != null ? e.popupFontColor : ModalPopupsDefaults.popupFontColor; e.footerBackColor = e.footerBackColor != null ? e.footerBackColor : ModalPopupsDefaults.footerBackColor; e.footerFontColor = e.footerFontColor != null ? e.footerFontColor : ModalPopupsDefaults.footerFontColor; c += " background-color:" + e.titleBackColor + ";"; c += " color:" + e.titleFontColor + ";"; l += " background-color:" + e.popupBackColor + ";"; l += " color:" + e.popupFontColor + ";"; v += " background-color:" + e.footerBackColor + ";"; v += " color:" + e.footerFontColor + ";"; var j = 0; if (ModalPopupsSupport.getLayerWidth(r.id) > j) { j = ModalPopupsSupport.getLayerWidth(r.id) } if (ModalPopupsSupport.getLayerWidth(d.id) > j) { j = ModalPopupsSupport.getLayerWidth(d.id) } if (ModalPopupsSupport.getLayerWidth(p.id) > j) { j = ModalPopupsSupport.getLayerWidth(p.id) } var b = ModalPopupsSupport.getLayerHeight(r.id) + ModalPopupsSupport.getLayerHeight(d.id) + ModalPopupsSupport.getLayerHeight(p.id); e.width = e.width != null ? e.width : (j + 4); e.height = e.height != null ? e.height : b; var o = ModalPopupsSupport.getLayerHeight(d.id); if (e.height > b) { o = e.height - ModalPopupsSupport.getLayerHeight(r.id) - ModalPopupsSupport.getLayerHeight(p.id); l += " height:" + o + "px;"; b = ModalPopupsSupport.getLayerHeight(r.id) + o + ModalPopupsSupport.getLayerHeight(p.id) } c += " top:1px;"; l += " top:" + ModalPopupsSupport.getLayerHeight(r.id) + "px;"; v += " top:" + (ModalPopupsSupport.getLayerHeight(r.id) + (o)) + "px;"; c += " width:" + (e.width - 10) + "px;"; l += " width:" + (e.width - 10) + "px;"; v += " width:" + (e.width - 10) + "px;"; var q = ModalPopupsSupport.getFrameWidth(); var g = ModalPopupsSupport.getFrameHeight(); if (e.height < b) { e.height = b } e.top = e.top != null ? e.top : ((g / 2) - (e.height / 2)); e.left = e.left != null ? e.left : ((q / 2) - (e.width / 2)); c += " left:1px;"; l += " left:1px;"; v += " left:1px;"; if (e.width) { u += " width:" + e.width + "px;" } else { u += " width:" + e.maxWidth + "px;" } if (e.height) { u += " height:" + (e.height - 1) + "px;" } else { u += " height:" + (b - 1) + "px;" } if (ModalPopupsSupport.isIE) { r.style.cssText = c; d.style.cssText = l; p.style.cssText = v } else { r.setAttribute("style", c); d.setAttribute("style", l); p.setAttribute("style", v) } e.shadow = e.shadow != null ? e.shadow : ModalPopupsDefaults.shadow; e.shadowSize = e.shadowSize != null ? e.shadowSize : ModalPopupsDefaults.shadowSize; if (e.shadow) { e.shadowSize = e.shadowSize != null ? e.shadowSize : ModalPopupsDefaults.shadowSize; e.shadowColor = e.shadowColor != null ? e.shadowColor : ModalPopupsDefaults.shadowColor; f += "background-color:" + e.shadowColor + ";"; if (e.width) { f += " width:" + e.width + "px;" } else { f += " width:" + maxWidth + "px;" } if (e.height) { f += " height:" + (e.height - 1) + "px;" } else { f += " height:" + (b) + "px;" } } else { f += " display:none;" } if (ModalPopupsSupport.isIE) { t.style.cssText = u; a.style.cssText = f; k.style.cssText = w } else { t.setAttribute("style", u); a.setAttribute("style", f); k.setAttribute("style", w) } if (!ModalPopupsSupport.isOlderIE()) { ModalPopupsSupport.centerElement(document.getElementById(n + "_background"), 0, true) } else { var s = ModalPopupsSupport.getViewportDimensions(); k.innerHTML = "<div><iframe style='z-index:-1; position:absolute; top:0;left:0 display:none; display/**/:block; position:absolute; filter:mask(); width:" + s.width + "px; height:" + s.height + "px;' id='corr_bug_ie' src='../common/imgLay/spinner.gif'></iframe></div>" } ModalPopupsSupport.centerElement(document.getElementById(n + "_popup"), 0, false); if (e.shadow) { ModalPopupsSupport.centerElement(document.getElementById(n + "_shadow"), e.shadowSize, false) } e.loadTextFile = e.loadTextFile != null ? e.loadTextFile : ""; if (e.loadTextFile != "") { ModalPopups._loadTextFile(n, e, h, e.loadTextFile) } window.onresize = function () { ModalPopupsSupport.centerElement(document.getElementById(n + "_background"), 0, true); ModalPopupsSupport.centerElement(document.getElementById(n + "_popup"), 0, false); if (e.shadow) { ModalPopupsSupport.centerElement(document.getElementById(n + "_shadow"), e.shadowSize, false) } }; window.onscroll = function () { ModalPopupsSupport.centerElement(document.getElementById(n + "_background"), 0, true); ModalPopupsSupport.centerElement(document.getElementById(n + "_popup"), 0, false); if (e.shadow) { ModalPopupsSupport.centerElement(document.getElementById(n + "_shadow"), e.shadowSize, false) } } }, _loadTextFile: function (e, d, c, a) { var b = ModalPopupsSupport.getXmlHttp(); b.open("GET", a, true); b.onreadystatechange = function () { if (b.readyState == 4) { var f = b.responseText.replace("\r\n", "<br>").replace("\n\r", "<br>").replace("\n", "<br>").replace("\r", "<br>"); var g = "<div style='overflow-y: scroll; position:absolute; top:5px; left:5px; height:" + (d.height - 65) + "px; width:" + (d.width - 10) + "px;'>"; g += f; g += "</div>"; ModalPopups.GetCustomControl(e + "_popupBody").innerHTML = g; d.loadTextFile = ""; ModalPopups._styleAllLayers(e, d, c) } }; b.send(null) } }; var ModalPopupsSupport = { isIE: function () { return (window.ActiveXObject) ? true : false }, isOlderIE: function () { var a = -1; if (navigator.appName == "Microsoft Internet Explorer") { var b = navigator.userAgent; var c = new RegExp("MSIE ([0-9]{1,}[.0-9]{0,})"); if (c.exec(b) != null) { a = parseFloat(RegExp.$1) } } if (a > -1 && a < 7) { return true } else { return false } }, makeLayer: function (d, b, c) { var a = document.createElement("div"); a.id = d; if (c) { c.appendChild(a) } else { document.body.appendChild(a) } return a }, deleteLayer: function (b) { var a = findLayer(b); if (a) { document.body.removeChild(a) } }, findLayer: function (a) { return document.all ? document.all[a] : document.getElementById(a) }, findControl: function (b, a) { if (a == null) { return document.all ? document.all[b] : document.getElementById(b) } else { return document.all ? document.all[b] : document.getElementById(b) } }, getLayerHeight: function (a) { if (document.all) { gh = document.getElementById(a).offsetHeight } else { gh = document.getElementById(a).offsetHeight } return gh }, getLayerWidth: function (a) { gw = document.getElementById(a).offsetWidth; return gw }, getViewportDimensions: function () { var b = 0, a = 0; if (self.innerHeight) { b = window.innerHeight; a = window.innerWidth } else { if (document.documentElement && document.documentElement.clientHeight) { b = document.documentElement.clientHeight; a = document.documentElement.clientWidth } else { if (document.body) { b = document.body.clientHeight; a = document.body.clientWidth } } } return { height: parseInt(b, 10), width: parseInt(a, 10)} }, getScrollXY: function () { var b = 0, a = 0; if (typeof (window.pageYOffset) == "number") { a = window.pageYOffset; b = window.pageXOffset } else { if (document.body && (document.body.scrollLeft || document.body.scrollTop)) { a = document.body.scrollTop; b = document.body.scrollLeft } else { if (document.documentElement && (document.documentElement.scrollLeft || document.documentElement.scrollTop)) { a = document.documentElement.scrollTop; b = document.documentElement.scrollLeft } } } return [b, a] }, centerElement: function (d, g, c) { var b = ModalPopupsSupport.getViewportDimensions(); var f = (b.width == 0) ? 50 : parseInt((b.width - d.offsetWidth) / 2, 10); var e = (b.height == 0) ? 50 : parseInt((b.height - d.offsetHeight) / 2, 10); var a = ModalPopupsSupport.getScrollXY(); if (!c) { d.style.left = (f + g) + "px" } d.style.top = (e + g + a[1]) + "px"; b, f, e, d = null }, readFile: function (c, a) { var b = getXmlHttp(); var d = c + "?r=" + Math.random(); b.open("GET", d, true); b.onreadystatechange = function () { if (b.readyState == 4) { a.innerHTML = b.responseText } }; b.send(null) }, getFrameWidth: function () { var a = document.documentElement.clientWidth; if (self.innerWidth) { a = self.innerWidth } else { if (document.documentElement && document.documentElement.clientWidth) { a = document.documentElement.clientWidth } else { if (document.body) { a = document.body.clientWidth } else { return } } } return a }, getFrameHeight: function () { var a = document.documentElement.clientHeight; if (self.innerWidth) { a = self.innerHeight } else { if (document.documentElement && document.documentElement.clientWidth) { a = document.documentElement.clientHeight } else { if (document.body) { a = document.body.clientHeight } else { return } } } return a }, getXmlHttp: function () { var a; try { a = new XMLHttpRequest() } catch (b) { try { a = new ActiveXObject("Msxml2.XMLHTTP") } catch (b) { try { a = new ActiveXObject("Microsoft.XMLHTTP") } catch (b) { alert("Your browser does not support AJAX!"); return false } } } return a } };

// JSON for old browsers
var JSON; if (!JSON) { JSON = {} } (function () { function f(n) { return n < 10 ? "0" + n : n } if (typeof Date.prototype.toJSON !== "function") { Date.prototype.toJSON = function (key) { return isFinite(this.valueOf()) ? this.getUTCFullYear() + "-" + f(this.getUTCMonth() + 1) + "-" + f(this.getUTCDate()) + "T" + f(this.getUTCHours()) + ":" + f(this.getUTCMinutes()) + ":" + f(this.getUTCSeconds()) + "Z" : null }; String.prototype.toJSON = Number.prototype.toJSON = Boolean.prototype.toJSON = function (key) { return this.valueOf() } } var cx = /[\u0000\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g, escapable = /[\\\"\x00-\x1f\x7f-\x9f\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g, gap, indent, meta = { "\b": "\\b", "\t": "\\t", "\n": "\\n", "\f": "\\f", "\r": "\\r", '"': '\\"', "\\": "\\\\" }, rep; function quote(string) { escapable.lastIndex = 0; return escapable.test(string) ? '"' + string.replace(escapable, function (a) { var c = meta[a]; return typeof c === "string" ? c : "\\u" + ("0000" + a.charCodeAt(0).toString(16)).slice(-4) }) + '"' : '"' + string + '"' } function str(key, holder) { var i, k, v, length, mind = gap, partial, value = holder[key]; if (value && typeof value === "object" && typeof value.toJSON === "function") { value = value.toJSON(key) } if (typeof rep === "function") { value = rep.call(holder, key, value) } switch (typeof value) { case "string": return quote(value); case "number": return isFinite(value) ? String(value) : "null"; case "boolean": case "null": return String(value); case "object": if (!value) { return "null" } gap += indent; partial = []; if (Object.prototype.toString.apply(value) === "[object Array]") { length = value.length; for (i = 0; i < length; i += 1) { partial[i] = str(i, value) || "null" } v = partial.length === 0 ? "[]" : gap ? "[\n" + gap + partial.join(",\n" + gap) + "\n" + mind + "]" : "[" + partial.join(",") + "]"; gap = mind; return v } if (rep && typeof rep === "object") { length = rep.length; for (i = 0; i < length; i += 1) { if (typeof rep[i] === "string") { k = rep[i]; v = str(k, value); if (v) { partial.push(quote(k) + (gap ? ": " : ":") + v) } } } } else { for (k in value) { if (Object.prototype.hasOwnProperty.call(value, k)) { v = str(k, value); if (v) { partial.push(quote(k) + (gap ? ": " : ":") + v) } } } } v = partial.length === 0 ? "{}" : gap ? "{\n" + gap + partial.join(",\n" + gap) + "\n" + mind + "}" : "{" + partial.join(",") + "}"; gap = mind; return v } } if (typeof JSON.stringify !== "function") { JSON.stringify = function (value, replacer, space) { var i; gap = ""; indent = ""; if (typeof space === "number") { for (i = 0; i < space; i += 1) { indent += " " } } else { if (typeof space === "string") { indent = space } } rep = replacer; if (replacer && typeof replacer !== "function" && (typeof replacer !== "object" || typeof replacer.length !== "number")) { throw new Error("JSON.stringify") } return str("", { "": value }) } } if (typeof JSON.parse !== "function") { JSON.parse = function (text, reviver) { var j; function walk(holder, key) { var k, v, value = holder[key]; if (value && typeof value === "object") { for (k in value) { if (Object.prototype.hasOwnProperty.call(value, k)) { v = walk(value, k); if (v !== undefined) { value[k] = v } else { delete value[k] } } } } return reviver.call(holder, key, value) } text = String(text); cx.lastIndex = 0; if (cx.test(text)) { text = text.replace(cx, function (a) { return "\\u" + ("0000" + a.charCodeAt(0).toString(16)).slice(-4) }) } if (/^[\],:{}\s]*$/.test(text.replace(/\\(?:["\\\/bfnrt]|u[0-9a-fA-F]{4})/g, "@").replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g, "]").replace(/(?:^|:|,)(?:\s*\[)+/g, ""))) { j = eval("(" + text + ")"); return typeof reviver === "function" ? walk({ "": j }, "") : j } throw new SyntaxError("JSON.parse") } } } ());

//------------------------ Spinner
Spinner = function (container, options) {

    // fetch the DOM node if a string was supplied
    if (typeof container == 'string') {
        container = document.getElementById(container);
    }

    // store the options and set the default values
    this.options = (options ? options : {});
    if (!('className' in this.options)) this.options.className = 'spinBox';
    if (!('step' in this.options)) this.options.step = 1;
    if (!('decimals' in this.options)) this.options.decimals = 0;

    this.lastLetter = options.lastLetter || null;

    // check whether the input field should be created
    var inputs = container.getElementsByTagName('input');
    if (inputs.length == 0) {

        // create the input node
        this.input = document.createElement('input');
        this.input.type = "text";
        this.input.maxLength = 14;
        this.input.id = container.id;
        this.setValue('value' in this.options ? this.options.value : 0, true);
        container.appendChild(this.input);

    } else {

        // store a reference to the input node
        this.input = inputs[0];
        if ('value' in this.options) this.setValue(this.options.value, true);

    }

    // create the up button
    //    var upButton = document.createElement('span');
    //    upButton.appendChild(document.createElement('span'));
    //    container.appendChild(upButton);

    // create the down button
    //    var downButton = document.createElement('span');
    //    downButton.appendChild(document.createElement('span'));
    //    container.appendChild(downButton);

    // apply the classes
    container.className += ' ' + this.options.className;
    //    upButton.className = this.options.className + 'Up';
    //    downButton.className = this.options.className + 'Down';
    //    if (this.lastLetter == Fanex.CustSetting.PT_LAST_LETTER) {
    //        // Set Right css
    //        upButton.style.right = downButton.style.right = ((options.minWidth / 2) - (Fanex.CustSetting.SPINNER_BOX_WIDTH / 2) - 10) + "px";
    //    }
    // add the listeners
    //this.addEventListener(this.input, 'mousewheel', this.handleMouseWheel, [], true);
    //this.addEventListener(this.input, 'DOMMouseScroll', this.handleMouseWheel, [], true);
    this.addEventListener(this.input, 'keydown', this.handleKeyDown, [], true);
    this.addEventListener(this.input, 'keypress', this.handleKeyPress, [], true);
    this.addEventListener(this.input, 'keyup', this.stop);
    //    this.addEventListener(upButton, 'mousedown', this.start, [true]);
    //    this.addEventListener(upButton, 'mouseup', this.stop);
    //    this.addEventListener(upButton, 'mouseout', this.stop);
    //    this.addEventListener(downButton, 'mousedown', this.start, [false]);
    //    this.addEventListener(downButton, 'mouseup', this.stop);
    //    this.addEventListener(downButton, 'mouseout', this.stop);

    return this;

}

Spinner.prototype = {
    getValue: function () {
        var currentTextValue = this.input.value;
        var currentValue = 0;
        if (this.lastLetter && currentTextValue.indexOf(this.lastLetter) > -1) {
            currentValue = currentTextValue.substring(0, currentTextValue.length - 1);
        }
        else {
            currentValue = currentTextValue;
        }
        var currentParseValue = parseFloat(currentValue);
        if (isNaN(currentParseValue)) currentParseValue = 0;
        return currentParseValue;
    },
    setValue: function (value, noCheckConstrain) {

        noCheckConstrain = noCheckConstrain || false;
        // ensure the value is within the permitted range
        if ('minimum' in this.options && !noCheckConstrain) value = Math.max(this.options.minimum, value);
        if ('maximum' in this.options && !noCheckConstrain) value = Math.min(this.options.maximum, value);

        // store the sign
        var sign = (value < 0 ? '-' : '');
        value = Math.abs(value);

        // determine the multiplier for rounding
        var multiplier = Math.pow(10, this.options.decimals);

        // split the value in to integer and fractional parts
        value = Math.round(value * multiplier);
        var integer = (value - value % multiplier) / multiplier;
        var fractional = '' + value % multiplier;

        // add leading zeros to the fractional part
        var decimalLength = this.options.decimals - fractional.length;
        for (var i = 0; i < decimalLength; i++) {
            fractional = '0' + fractional;
        }


        if (fractional.length > 0) {
            while (fractional.charAt(fractional.length - 1) == '0') {
                fractional = fractional.substring(0, fractional.length - 1);
            }
        }

        // set the value
        this.input.value = sign + integer + ((this.options.decimals > 0 && fractional.length > 0) ? '.' + fractional : '') + (this.lastLetter != null ? this.lastLetter : '');
    },
    addEventListener: function (node, event, listener, parameters, allowDefault) {

        // store a reference to the 'this' object
        var thisObject = this;

        // create the bound listener
        var boundListener = function (e) {

            // create the list of parameters
            var callParameters = [e ? e : window.event].concat(parameters);

            // call the listener
            listener.apply(thisObject, callParameters);

            // prevent the default action if necessary
            if (!allowDefault) {
                if (e && e.preventDefault) {
                    e.preventDefault();
                } else {
                    window.event.returnValue = false;
                }
            }
        }

        // add the event listener
        if (node.addEventListener) {
            node.addEventListener(event, boundListener, false);
        } else {
            node.attachEvent('on' + event, boundListener);
        }
    },
    bind: function (f, parameters) {

        // store a reference to the 'this' object
        var thisObject = this;

        // return the bound function
        return function () {
            f.apply(thisObject, parameters ? parameters : []);
        };
    },
    handleMouseWheel: function (e) {

        // check whether the field is active
        if (document.activeElement == this.input) {

            // update the value
            if (e.wheelDelta) {
                this.start(e, e.wheelDelta > 1);
            } else if (e.detail) {
                this.start(e, e.detail < 1);
            }
            this.stop();

            // prevent the default action
            if (e.preventDefault) {
                e.preventDefault();
            } else {
                window.event.returnValue = false;
            }
        }
    },
    handleKeyDown: function (e) {

        // if the up or down keys were pressed, start updating
        if (e.keyCode == 38) this.start(e, true);
        if (e.keyCode == 40) this.start(e, false);
    },
    handleKeyPress: function (e) {

        // determine the character code
        var charCode = ('charCode' in e ? e.charCode : e.keyCode);

        // allow special key presses
        if (charCode == 0 || e.altKey || e.ctrlKey || e.metaKey) return;

        // allow a minus sign if the value can be negative
        if (charCode == 45 && (!('minimum' in this.options) || this.options.minimum < 0)) {
            return;
        }

        // allow a decimal point if the value may contain decimals
        if (charCode == 46 && this.options.decimals > 0) return;

        // allow digits
        if (charCode >= 48 && charCode <= 57) return;

        // prevent the default action
        if (e.preventDefault) {
            e.preventDefault();
        } else {
            window.event.returnValue = false;
        }
    },
    start: function (e, up) {
        // Return if input is disable
        if (this.input.disabled == true) return;

        // if we are already updating, return immediately
        if (this.timeout) return;

        // set the update step
        this.updateStep = (up ? this.options.step : -this.options.step);

        // initialise the timeout delay
        this.timeoutDelay = 500;

        // update the value
        this.update();
    },
    stop: function () {
        //// Check valid input
        var value = this.getValue();
        if (isNaN(value)) this.setValue(0);

        if (this.input.disabled == true) return;
        // clear the timeout if it exists
        if (this.timeout) window.clearTimeout(this.timeout);
        this.timeout = null;
    },
    update: function () {

        // determine the current value
        var value = this.getValue();
        if (isNaN(value)) value = 0;

        // update the value
        this.setValue(value + this.updateStep);

        // call this function again after a reduced delay
        this.timeoutDelay = Math.max(20, Math.floor(this.timeoutDelay * 0.9));
        this.timeout = window.setTimeout(this.bind(this.update), this.timeoutDelay);

        //// Trigger OnChange
        if (this.input.onchange) this.input.onchange();
    },
    updateAndSetMaxValue: function (maxValue) {
        if (this.options.minimum > maxValue) maxValue = this.options.minimum;
        if (maxValue < 0) maxValue = 0;
        this.options.maximum = maxValue;
        this.setValue(maxValue);
    },
    updateMaxValue: function (maxValue) {
        if (this.options.minimum > maxValue) maxValue = this.options.minimum;
        if (maxValue < 0) maxValue = 0;
        this.options.maximum = maxValue;
    },
    updateMinValue: function (mixValue) {
        if (mixValue < 0) mixValue = 0;
        this.options.minimum = mixValue;
    },
    isValid: function (hideMessage) {
        var value = this.getValue();
        if (!this.digitsValid(value)) {
            if (hideMessage == true) {
            }
            else {
                Fanex.ValidateMessage(this.input, Language.INVALID_INPUT_DECIMAIL_POINT, -50, 65);
            }
            return false;
        }
        if ((value < this.options.minimum || value > this.options.maximum) && (this.options.minimum <= this.options.maximum)) {
            if (hideMessage == true) {
            }
            else {
                Fanex.ValidateMessage(this.input, Language.INVALID_RANGE.replace('{0}', this.options.minimum).replace('{1}', this.options.maximum), -50, 65);
            }
            return false;
        }
        return true;
    },
    digitsValid: function (value) {
        // determine the multiplier for rounding
        var valueString = value.toString();

        if (valueString.indexOf('.') > -1) {
            if (valueString.indexOf('.') + 3 < valueString.length) {
                return false;
            }
        };
        return true;
    },
    digistIsValid: function (hideMessage) {
        var value = this.getValue();
        if (!this.digitsValid(value)) {
            if (hideMessage == true) {
            }
            else {
                Fanex.ValidateMessage(this.input, Language.INVALID_INPUT_DECIMAIL_POINT, -50, 65);
            }
            return false;
        }
        return true;
    },
    alertRangeMessage: function () {
        Fanex.ValidateMessage(this.input, Language.INVALID_RANGE.replace('{0}', this.options.minimum).replace('{1}', this.options.maximum), -50, 65);
    },
    alertDigitsInvalid: function () {
        Fanex.ValidateMessage(this.input, Language.INVALID_INPUT_DECIMAIL_POINT, -50, 65);
    }
};
