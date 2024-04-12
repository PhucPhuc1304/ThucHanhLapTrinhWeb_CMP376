function fetchProducts() {
    fetch('https://localhost:7231/api/products')
        .then(response => response.json())
        .then(data => {
            const productList = document.getElementById('productList');
            productList.innerHTML = '';
            data.forEach(product => {
                productList.innerHTML += `
                    <tr>
                        <td>${product.id}</td>
                        <td>${product.name}</td>
                        <td>${product.price}</td>
                        <td>${product.description}</td>
                        <td>
                            <button class="btn btn-danger" data-id="${product.id}" onclick="deleteProduct(event)">Delete</button>
                            <button class="btn btn-warning" data-id="${product.id}" onclick="editProduct(event)">Edit</button>
                            <button class="btn btn-primary" data-id="${product.id}" onclick="viewDetails(event)">View</button>
                        </td>
                    </tr>
                `;
            });
        })
        .catch(err => {
            console.log(err);
        });
}

function editProduct(event) {
    const productId = event.target.dataset.id;
    console.log(`Editing product with ID ${productId}`);
    fetch(`https://localhost:7231/api/products/${productId}`)
        .then(response => response.json())
        .then(book => {
            document.getElementById('editBookName').value = book.name;
            document.getElementById('editPrice').value = book.price;
            document.getElementById('editDescription').value = book.description;
            $('#modalEditBookInfo').modal('show');
            document.getElementById('btnEditChanges').addEventListener('click', function() {
                const editedBook = {
                    id: productId, 
                    name: document.getElementById('editBookName').value,
                    price: document.getElementById('editPrice').value,
                    description: document.getElementById('editDescription').value
                };
                fetch(`https://localhost:7231/api/products/${editedBook.id}`, {
                    method: 'PUT',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(editedBook)
                })
                .then(response => {
                    if (response.ok) {
                        console.log('Book information updated successfully');
                        $('#modalEditBookInfo').modal('hide');
                        location.reload();

                    } else {
                        throw new Error('Failed to update book information');
                    }
                })
                .catch(error => {
                    console.error('Error updating book information:', error);
                });
            });
        })
        .catch(error => {
            console.error('Error fetching book details:', error);
        });
}


function deleteProduct(event) {
    const productId = event.target.dataset.id;
    
    const modalBody = document.querySelector('#deleteModal .modal-body');
    modalBody.innerHTML = `Bạn có muốn xóa sách có ID ${productId}?`;
    $('#deleteModal').modal('show');

    document.getElementById('confirmDelete').addEventListener('click', function() {
        fetch(`https://localhost:7231/api/products/${productId}`, {
            method: 'DELETE'
        })
        .then(response => {
            if (response.ok) {
                alert(`Product with ID ${productId} has been deleted successfully.`);
            } else {
                alert(`Failed to delete product with ID ${productId}.`);
            }
            location.reload();

        })
        .catch(error => {
            console.error('Error deleting product:', error);
            location.reload();

        });
        $('#deleteModal').modal('hide');
    });
}


function viewDetails(event) {
    const productId = event.target.dataset.id;
    fetch(`https://localhost:7231/api/products/${productId}`)
        .then(response => response.json())
        .then(product => {
            // Update elements with product data
            const idElement = document.querySelector('[data-atr="id"]');
            const nameElement = document.querySelector('[data-atr="bookname"]');
            const nameElementAlt = document.querySelector('[data-atr="bookname_"]');
            const descriptionElement = document.querySelector('[data-atr="description"]');
            
            idElement.textContent = product.id;
            nameElement.textContent = product.name;
            nameElementAlt.textContent = product.name;
            descriptionElement.textContent = product.description;
            $('#modalViewDetailInfo').modal('show');
        })
        .catch(err => {
            alert(err);
        });
}

document.getElementById('btnAdd').addEventListener('click', function() {
    const bookName = document.getElementById('bookName').value;
    const price = document.getElementById('price').value;
    const description = document.getElementById('description').value;

    if (bookName.trim() === '' || price.trim() === '' || description.trim() === '') {
        alert('Vui lòng nhập đầy đủ thông tin sản phẩm.');
        return;
    }

    const newProduct = {
        name: bookName,
        price: parseFloat(price),
        description: description
    };

    fetch('https://localhost:7231/api/products', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(newProduct)
    })
    .then(response => {
        if (response.ok) {
            alert('Thêm sản phẩm mới thành công.');
        } else {
            alert('Thêm sản phẩm mới thất bại.');
        }
        location.reload();

    })
    .catch(error => {
        alert('Lỗi khi thêm sản phẩm mới:', error);
    });
});
fetchProducts();
