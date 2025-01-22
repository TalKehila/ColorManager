
        // validtions with regex 
    function validateHebrewOnly(input) {
            return /^[\u0590-\u05FF\s]+$/.test(input);
        }

    function validatePositiveDecimal(input) {
            var val = parseFloat(input);
            return (!isNaN(val) && val >= 0);
        }

    function validatePositiveInteger(input) {
            var val = parseInt(input);
            return (!isNaN(val) && val >= 0);
        }

    function validateAddForm() {
            var colorName = $('#<%=txtColorName.ClientID%>').val().trim();

    var price = $('#<%=txtPrice.ClientID%>').val().trim();
    var displayOrder = $('#<%=txtDisplayOrder.ClientID%>').val().trim();

    if (!validateHebrewOnly(colorName)) {
        alert("שם הצבע חייב להיות בעברית בלבד.");
    return false;
            }
    if (!validateHexCode(colorHex)) {
        alert("קוד הצבע חייב להיות בפורמט Hex תקין, לדוגמה: #FFF או #FFFFFF.");
    return false;
            }
    if (!validatePositiveDecimal(price)) {
        alert("מחיר חייב להיות מספר חיובי.");
    return false;
            }
    if (!validatePositiveInteger(displayOrder)) {
        alert("סדר הצגה חייב להיות מספר שלם חיובי.");
    return false;
            }
    return true;
        }

    // Drag-and-drop 
    $(function () {
        $("#<%= gvColors.ClientID %> tbody").sortable({
            items: "tr:not(:first-child)", // cant move the topics of the tables 
            cursor: "move",
            opacity: 0.6,
            update: function (event, ui) {
                var order = [];
                $("#<%= gvColors.ClientID %> tbody tr").each(function (index) {
                    var id = $(this).find(".color-id").text();
                    order.push({ ID: id, DisplayOrder: index + 1 });
                });

                // sending new order to the server! 
                $.ajax({
                    type: "POST",
                    url: "Colors.aspx/UpdateDisplayOrder",
                    data: JSON.stringify({ order: order }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function () {
                        alert("סדר עודכן בהצלחה!");
                        location.reload(); // refresh the pagee
                    },
                   
                });
            }
        }).disableSelection();
       });
