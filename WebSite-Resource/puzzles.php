<?php 
	$colors = array(
		"White"			=> "#FFFFFF",
		"Peach"			=> "#F5D4B4",
		"Salmon"		=> "#FFD3CB",
		"Pink"			=> "#FCA8CC",
		"Red" 			=> "#C91111",
		"Red-Orange" 	=> "#D84E09",
		"Light Orange"	=> "#FED8B1",
		"Orange" 		=> "#FF8000",
		"Yellow-Orange"	=> "#FD9800",
		"Lemon Yellow"	=> "#F4FA9F",
		"Yellow" 		=> "#F6EB20",
		"Golden Yellow" => "#F6E120",
		"Bronze Yellow" => "#A78B00",	
		"Yellow-Green" 	=> "#51C201",
		"Green" 		=> "#1C8E0D",
		"Jade Green"	=> "#7E9156",
		"Light Blue"	=> "#83AFDB",
		"Sky Blue" 		=> "#09C5F4",
		"Aqua Green"	=> "#5BD2C0",
		"Turquoise"		=> "#17BFDD",
		"Green-Blue"	=> "#098FAB",
		"Pine-Green"	=> "#007872",
		"Blue" 			=> "#2862B9",
		"Violet" 		=> "#7E44BC",
		"Raspberry"		=> "#AA0570",
		"Mahogany"		=> "#B44848",
		"Maroon"		=> "#A32E12",
		"Magenta"		=> "#F863CB",
		"Tan"			=> "#CC8454",
		"Light Brown" 	=> "#BF6A1F",
		"Brown" 		=> "#943F07",
		"Dark Brown"	=> "#514E49",
		"Gray"			=> "#808080",
		"Slate"			=> "#7C7C99",
		"Cool Gray"		=> "#788193",
		"Black" 		=> "#000000"
	);
	$palet = "<table><thead><tr><td colspan='6' class='current_color'> White </td></tr></thead><tbody><tr>";
	$a = 0;
	foreach ($colors as $color => $hex)
	{
		if ($a%6 == 0)
		{
			$palet .= "</tr><tr>";	
		}
		$palet .= "<td class='palet-color' style='background-color:$hex;' title='$color' onClick='setColor(\"$hex\", \"$color\", false)'> </td>";
		$a++;
	}
	$palet .= "</tr>".
			"</tbody>" .
			"<tfoot>" .
				"<tr>" .
					"<td class='eraser'" .
						"colspan='6'" .
						"onClick='setColor(\"#FFFFFF\", \"~Erase Mode On~\", true)'>".
							"Eraser" .
					"</td>" . 
				"</tr>" . 
			"</tfoot>" .
		"</table>";
?>
<!doctype html>
<html>
<head>
	<meta charset="utf-8">
	<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
	<title>Zombie Kitty Asset Creation Site - Puzzle Generator</title>
    <link rel="stylesheet" type="text/css" href="css/style.css"> 
</head>

<body>
	<h1>Puzzle Generator</h1>

	<h2>Step 1:</h2>
	<p>Select your puzzle dimensions</p>
    <form id="puzzle_size">
    	<label>Puzzle Size by Level</label>
        <select id="size" name="size">
        <?php 
			$sizes = array(
				"5x5" => "Level 1 Puzzles (5x5)",
				"5x10" => "Level 2 Puzzles (5x10)",
				"10x10" => "Level 3 Puzzles (10x10)",
				"10x15" => "Level 4 Puzzles (10x15)",
				"15x15" => "Level 5 Puzzles (15x15)",
				"15x20" => "Level 6 Puzzles (15x20)",
				"20x20" => "Level 7 Puzzles (20x20)",
				"30x30" => "Level 8 Puzzles (30x30)",
				"40x40" => "Level 9 Puzzles (40x40)"
			);
			foreach($sizes as $key => $value)
			{
				echo "<option value='$key'>$value</option>";	
			}
		?>
        </select>
        <button name="submit" type="submit">Create Puzzle</button>
    </form>
    <h2>Step 2:</h2>
    <p>Design your puzzle</p>
    <div class="palet">
		<?php echo $palet; ?>
	</div>
    <div class="workarea"></div>
    <h2>Step 3:</h2>
    <p>Generate your XML code</p>
    <form id="xml">
    	<label>Puzzle Name</label><input type="text" name="name" id="xml-name"> <br />
        <label>Puzzle Number</label><input type="text" name="number" id="xml-num"> <br />
	    <button name="submit" type="submit">Generate XML</button>
        	<br />
    	<label>Text Area</label>
        	<br />
        <textarea id="result"></textarea>
    </form>
	<a href="/">Home</a>
    
	<script type="text/javascript">
		
		var paint = "#FFFFFF";
		var eraserMode = false;
		function setColor(color, text, eraserOn){
			$(".current_color").css("background-color", color);
			$(".current_color").html(text);	
			paint = color;
			eraserMode = eraserOn;
		}
		function colorCell(elem){
			
			if (eraserMode)
			{
				$(elem).html("x");
				$(elem).removeAttr("style");	
			}
			else
			{
				$(elem).html(" ");
				$(elem).css("background-color", paint);
			}
		}
		
		$("form#puzzle_size").submit(function (event) {
			event.preventDefault();
			if (confirm("This will erase all your progress. " + 
									 "This cannot be undone.", 'Alert Dialog'))
			{
				var xy = $("select#size").val();
				var x = parseInt(xy);
				var y = parseInt(xy.substr(xy.indexOf("x")+1));
				var layout = "<table>";
				for (var i = 0; i < x; i++){
					layout += "<tr>";
					
					for (var j = 0; j < y; j++){
						layout += "<td class='cell' id='" + i + "x" + j +"' onClick='colorCell(this)'>x</td>"
					}
					
					layout += "</tr>";
				}
				layout += "</table>";
				$("div.workarea").html(layout);	
			}
		});
		
		$("form#xml").submit(function(event) {
			event.preventDefault();
			var xnum = $("input#xml-num").val();
			var xname = $("input#xml-name").val();
			var xy = $("select#size").val();
			var x = parseInt(xy);
			var y = parseInt(xy.substr(xy.indexOf("x")+1));
			var output = "%3Cpuzzle%20name%3D%22" + xname + "%22%20puzzlenum%3D%22" + xnum + "%22%3E%0A";
			for (i = 0; i < x; i++){
				output += "%3Crow%3E%0A";
				for (j = 0; j < y; j++){
					if ($("td#" + i + "x" + j).html() == "x") {
						output += "%3Ccell%3E" + "false" + "%3C/cell%3E%0A";
					}else{
						output += "%3Ccell%20color%3D%22" + $("td#" + i + "x" + j).css("background-color") + "%22%3E" + "true" + "%3C/cell%3E%0A";
					}
				}
				output += "%3C/row%3E%0A";
			}
			output += "%3C/puzzle%3E%0A";
			$("textarea#result").val(unescape(output));
		});
	</script>

</body>
</html>