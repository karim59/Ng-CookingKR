
//remplissage des dropdownlist
$(function () {
    $.ajax({
        type: "GET",
        url: "/Recipes/GetCategory",
        datatype: "Json",
        success: function (data) {
            $.each(data, function (index, value) {
                $('#dropdownCategory').append('<option value="' + value + '">' + value + '</option>');
            });
        }
    });

    $('#dropdownCategory').change(function () {

        $('#dropdownIngredient').empty();
        $.ajax({
            type: "POST",
            url: "/Recipes/GetIngredientByCategoryId",
            datatype: "Json",
            data: { categoryName: $('#dropdownCategory').val() },
            success: function (data) {
                $.each(data, function (index, value) {
                    $('#dropdownIngredient').append('<option value="' + value + '">' + value + '</option>');
                });
            }
        });
    });
});




//$(function () {
//    $('#ing').delegate("li", "click",
//        function () {
//            //$(this).remove();
//            //
//            var k = $(this).val()
//            $('#removedIngredient').val('jjjjj');
//        }
//        )
//});

//suppression des images d'ingredients
$(function(){
    $('#ing').delegate("button","click",
        function () {
            $(this).remove();
            var k= $(this).val()
            $('#RemovedIngredients').val($('#RemovedIngredients').val() + ';' + k);
            //ingremov
            //$('#cal').replaceWith('<input type="hidden" id="nbCalorie" name="nbCalorie" value="45">'
            //                   + '<span class="val" id="cal">  45 <span class="unit">kcal</span></span>')
        }      
        )
});

$(function () {
    $('#ing').delegate("li", "click",
        function () {
            $(this).remove();
            
        }
        )
});
$(function () {

    $('#bt').click(function () {
        $.ajax({
            type: "POST",
            url: "/Recipes/GetImgIngredient",
            datatype: "JSON",
            data: { nameIngredient: $('#dropdownIngredient').val(), nbCalor: $('#nbCalorie').val() }
        })
            .done(function(data) {
                console.log("data : " + data);
                $('#ing').append('<li class="item">' + 
                    '<img ' + 'class="item-img" src="/img/ingredients/'+data.IngredientImage+ '" id="'+data.Name+'" alt="'+data.Name+'"> '
                    + '<p class="item-name" title="raisin">' + data.Name + '</p>'
                    + '<button type="button" class="remove_ingredient" value="'+data.Name+'" ><span class="glyphicon glyphicon-remove"></span></button>'
                    +'</li>'
                    );
                
                $('#AddedIngredients').val($('#AddedIngredients').val() + ';' + data.Name);
                $('#AddedIngredients').append('kkk');
                $('#cal').replaceWith('<input type="hidden" id="nbCalorie" name="nbCalorie" value="' + data.Calorie + '">'
                               + '<span class="val" id="cal">' + data.Calorie + '<span class="unit">kcal</span></span>'
                   )
            })
            .fail(function() {
                alert( "error" );
            })
        ;
    });
});

$(function () {


});

var GetIngredientElements = function () {

    


};