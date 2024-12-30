var url = "https://localhost:44373/v1.0/Products/products";

var productList = document.getElementById("Products-list");
if (productList) {
    fetch(url)
        .then(Response => Response.json)
        .then(data => showProducts(data))
        .catch(ex => {
            alert("something went wrong");
            console.log(ex);
        });
}

function showProducts(products) {
    products.forEach(products => {
        let li = document.createElement("li");
        let text = `${products.name}  ($${products.price})`;
        li.appendChild(document.createTextNode(text));
        productList.appendChild(li);
    })
}