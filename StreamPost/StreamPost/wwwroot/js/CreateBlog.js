$(document).ready(function () {
    tinymce.init({
        selector: '#Content',
        height: 400,
        plugins: 'advlist autolink lists link image charmap preview anchor searchreplace visualblocks code fullscreen insertdatetime media table code help wordcount',
        toolbar: 'undo redo | formatselect | bold italic underline | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image media | removeformat | code',
        menubar: true,
        branding: false,
        content_style: "body { font-family: Arial, sans-serif; font-size: 16px; }",
        images_upload_handler: function (blobInfo) {
            return new Promise((resolve, reject) => {
                var reader = new FileReader();
                reader.readAsDataURL(blobInfo.blob());
                reader.onload = function () {
                    resolve(reader.result);
                };
                reader.onerror = function () {
                    reject("Error occured while reading the image file : " + error);
                }
            })
        },
        media_upload_handler: function (blobInfo) {
            return new Promise((resolve, reject) => {
                var reader = new FileReader();
                reader.readAsDataURL(blobInfo.blob());
                reader.onload = function () {
                    resolve(reader.result);
                };
                reader.onerror = function () {
                    reject("Error occured while reading the video file : " + error)
                }
            });
        }
    });

    document.addEventListener('focusin', (e) => {
        if (e.target.closest(".tox-tinymce, .tox-tinymce-aux, .moxman-window, .tam-assetmanager-root") !== null) {
            e.stopImmediatePropagation();
        }
    });

    let cropper;

    // Hide the cropper control buttons initially
    const cropButton = document.getElementById('cropButton');
    const rotateButton = document.getElementById('rotateButton');
    cropButton.style.display = 'none';
    rotateButton.style.display = 'none';

    // Handle the image selection for cropping
    document.getElementById('FeaturedImagePicker').addEventListener('change', function (event) {
        const file = event.target.files[0];

        if (file) {
            // Show the crop and rotate buttons once the image is selected
            cropButton.style.display = 'inline-block';
            rotateButton.style.display = 'inline-block';

            // Create a URL for the selected image file
            const imageURL = URL.createObjectURL(file);

            // Set the image src to display the selected image
            const previewImage = document.getElementById('previewImage');
            previewImage.src = imageURL;
            previewImage.style.display = 'block';

            // Initialize Cropper.js on the selected image
            cropper = new Cropper(previewImage, {
                //aspectRatio: 16 / 9,
                viewMode: 1,
                autoCropArea: 0.5,
                responsive: true
            });
        }
    });

    // Add crop functionality to the crop button
    cropButton.addEventListener('click', function (event) {
        event.preventDefault();  // Prevent form submission or page jump
        if (cropper) {
            const canvas = cropper.getCroppedCanvas();
            const croppedPreview = document.getElementById('croppedPreview');
            croppedPreview.src = canvas.toDataURL();

            // Hide the cropper control buttons after cropping
            cropButton.style.display = 'none';
            rotateButton.style.display = 'none';

            // Optionally, hide the image preview and cropper canvas
            const previewImage = document.getElementById('previewImage');
            previewImage.style.display = 'none';  // Hide the original image after crop
            $("#FeaturedImage").val(canvas.toDataURL());
            // Optionally, destroy the cropper instance (if you want to disable further cropping)
            cropper.destroy();
            cropper = null;
        }
    });

    // Add rotate functionality to the rotate button
    rotateButton.addEventListener('click', function (event) {
        event.preventDefault();  // Prevent form submission or page jump
        if (cropper) {
            cropper.rotate(90); // Rotate by 90 degrees (you can adjust this value)
        }
    });
});
