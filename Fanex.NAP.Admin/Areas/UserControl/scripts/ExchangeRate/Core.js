if (typeof (Fanex) == 'undefined') { // Require Fanex namespace
	Fanex = {};
	Fanex.CloneObject = function (obj) {
		var newObj = (obj instanceof Array) ? [] : {};
		for (i in obj) {
			if (i == 'clone') {
				continue;
			}
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
		var x1 = x[0];
		var x2 = x.length > 1 ? '.' + x[1] : String.Empty;
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
		if (number == String.Empty) {
			return 0;
		}

		if (isNaN(number)) {
			number = String.Empty;
		}

		for (var i = 0; i < Math.floor((number.length - (1 + i)) / 3); i++) {
			number = number.substring(0, number.length - (4 * i + 3)) + ',' + number.substring(number.length - (4 * i + 3));
		}

		return number;
	};
	// Format input number on event e, using: onkeyup=Fanex.FormatNumber(event)
	Fanex.FormatNumber = function (event, isFloat, length) {
		var evt = event ? event : window.event; // Fix for IE7

		if (evt.keyCode === 9) {
			return true; // Ignore TAB key
		}

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

		if (num.length > value.length) {
			caretPosition = caretPosition + 1;
		}
		Fanex.SetCaretPosition(obj, caretPosition);
		return true;
	};

	Fanex.FormatNumberAndDot = function (event, isFloat) {
		var evt = event ? event : window.event; // Fix for IE7
		if (evt.keyCode === 9 || evt.keyCode === 110 || evt.keyCode === 190) {
			return true; // Ignore TAB key
		}

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

		if (num.length > value.length) {
			caretPosition = caretPosition + 1;
		}
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
		if (charCode == 0 || e.altKey || e.ctrlKey || e.metaKey) {
			return;
		}

		// allow digits
		if (charCode >= 48 && charCode <= 57) {
			return;
		}

		// prevent the default action
		if (e.preventDefault) {
			e.preventDefault();
		} else {
			window.event.returnValue = false;
		}
	};

	Fanex.HandleKeyDownInputNumberAndDot = function (e) {
		e = e || event;
		var charCode = ('charCode' in e ? e.charCode : e.keyCode);
		var value = $(this).val();

		if (value.indexOf('.') == -1 && charCode == 46) {
			return;
		}

		// allow special key presses
		if (charCode == 0 || e.altKey || e.ctrlKey || e.metaKey) {
			return;
		}

		// allow digits
		if (charCode >= 48 && charCode <= 57) {
			return;
		}

		// prevent the default action
		if (e.preventDefault) {
			e.preventDefault();
		} else {
			window.event.returnValue = false;
		}
	};

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
		else if (oField.selectionStart || oField.selectionStart == '0') {
			iCaretPos = oField.selectionStart;
		}

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
	};

	Fanex.ValidateMessage = function (elementInput, errorMessage, xpos, pointerx) {
		var isXPos = (typeof (xpos) != "undefined" && xpos != null);
		if (isXPos && pointerx) {
			Validators.show(elementInput, errorMessage, xpos, pointerx, -45, 38, 18, 5000, false);
		}
		else {
			Validators.show(elementInput, errorMessage, -150, 210, -45, 38, 18, 5000, false);
		}
		Validators.animate(elementInput);
	};
}
