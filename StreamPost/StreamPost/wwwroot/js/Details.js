$(document).ready(function () {
    $("#CommentForm").on("submit", function (e) {
        e.preventDefault();
        var formData = $(this).serialize();
        var commentText = $("textarea[name='CommentText']").val().trim();

        if (commentText === "") {
            showToast("Please enter a comment!", "dark");
            return;
        }
        $.ajax({
            url: "/Home/Comment",
            method: "POST",
            data: formData,
            success: function (response) {
                if (response.success) {
                    $("#CommentForm")[0].reset();

                    $("#commentsContainer").append(`
                        <div class="comment-box">
                            <div class="d-flex justify-content-between">
                                <strong>${response.username}</strong>
                            </div>
                            <p class="mt-2">${response.commentText}</p>
                        </div>
                    `);

                    $("#CommentBtn").html(`<i class="bi bi-chat-dots"></i> ${response.commentCount} Comments`);

                    $(".comment-section:contains('No comments yet.')").hide();
                    showToast("Comment added", "success");
                } else {
                    showToast(response.message, "danger");
                }
            },
            error: function () {
                showToast("An error occurred while adding the comment.", "danger");
            }
        });
    });

    $("#LikeBtn").on("click", function (e) {
        e.preventDefault();
        var postid = parseInt($(this).attr("data-postid"));

        $.ajax({
            url: "/Home/Like",
            method: "POST",
            data: { postid: postid },
            success: function (response) {
                if (response.success) {
                    if (response.isLiked == null) {
                        $("#LikeBtn").removeClass("btn-outline-danger").addClass("btn-danger");
                    } else {
                        $("#LikeBtn").removeClass("btn-danger").addClass("btn-outline-danger");
                    }
                    $("#LikeBtn").html(`<i class="bi bi-heart-fill"></i> ${response.likeCount} Likes`);
                }
                else {
                    showToast("Something went wrong","danger");
                }
            },
            error: function (error) {
                showToast(error.message,"danger");
            }
        });
    });

    function showToast(message, type) {
        var toastHTML = `
        <div class="toast align-items-center border-0 bg-${type} mt-2" role="alert">
            <div class="d-flex">
                <div class="toast-body text-white">${message}</div>
                <button type="button" class="btn-close me-2 m-auto" data-bs-dismiss="toast" style="filter: brightness(0) invert(1);"></button>
            </div>
        </div>
    `;

        $("#toastContainer").append(toastHTML);
        var toast = new bootstrap.Toast($("#toastContainer .toast").last()[0]);
        toast.show();

        setTimeout(function () {
            $("#toastContainer .toast").first().remove();
        }, 4000);
    }


    $(document).ready(function () {
        $(".btn-outline-primary").click(function () {
            $('html, body').animate({
                scrollTop: $("#CommentForm").offset().top
            }, 100);
        });
    });
});