function checkLength() {
    var textbox = document.getElementById("searchTerm");
    if (textbox.value.length <= 2) {
        alert("Please enter minimum 3 characters");
        return false;
    }
    return true;
}

// Array to store selected question IDs
var selectedQuestionIds = [];

// Function to get the maximum allowed questions from form data
function getMaxAllowedQuestions() {
    var maxQuestions = 0;


    if (window.buildExamFormData && window.buildExamFormData.NoofQuestions) {
        maxQuestions = parseInt(window.buildExamFormData.NoofQuestions) || 0;

    }

    //console.log('Final maxQuestions:', maxQuestions);
    return maxQuestions;
}

// Function to add question to the list
function addToQuestionList(questionId) {
    // Convert to string to ensure consistent comparison
    questionId = questionId.toString();

    // Debug logging
    //console.log('Adding question:', questionId);

    // Get maximum allowed questions
    var maxQuestions = getMaxAllowedQuestions();

    // Check if NoofQuestions is not set (blank or empty) - give error immediately
    if (window.buildExamFormData &&
        (!window.buildExamFormData.NoofQuestions ||
            window.buildExamFormData.NoofQuestions.toString().trim() === '' ||
            window.buildExamFormData.NoofQuestions.toString().trim() === '0')) {
        alert('Cannot add questions. Number of questions limit is not set. Please go back and set the number of questions for the exam.');
        return false;
    }

    // Check if question is already in the list
    if (selectedQuestionIds.indexOf(questionId) !== -1) {
        return false;
    }

    // Check if adding this question would exceed the limit (only if maxQuestions > 0)
    if (maxQuestions > 0 && selectedQuestionIds.length >= maxQuestions) {
        alert('Cannot add more questions. Maximum allowed questions: ' + maxQuestions +
            '\n Currently selected: ' + selectedQuestionIds.length +
            '\n Please remove some questions before adding new ones.');
        return false;
    }

    // Add the question to the list
    selectedQuestionIds.push(questionId);
    updateSelectedCount();

    // Also check the corresponding checkbox if it exists
    var checkbox = $('input[name="selectedQuestions"][value="' + questionId + '"]');
    if (checkbox.length) {
        checkbox.prop('checked', true);
    }

    return true;
}
// Function to remove question from the list
function removeFromQuestionList(questionId) {
    // Convert to string to ensure consistent comparison
    questionId = questionId.toString();
    var index = selectedQuestionIds.indexOf(questionId);
    if (index > -1) {
        selectedQuestionIds.splice(index, 1);
        updateSelectedCount();

        // Also uncheck the corresponding checkbox if it exists
        var checkbox = $('input[name="selectedQuestions"][value="' + questionId + '"]');
        if (checkbox.length) {
            checkbox.prop('checked', false);
        }
    }
}

// Function to update the selected count display
function updateSelectedCount() {
    var countElement = document.getElementById('selectedCount');
    var submitButton = document.getElementById('submitSelectedQuestions');
    var maxQuestions = getMaxAllowedQuestions();

    if (selectedQuestionIds.length > 0) {
        var countText = selectedQuestionIds.length + ' question(s) selected';

        // Add limit information if available
        if (maxQuestions > 0) {
            countText += ' (Max: ' + maxQuestions + ')';
        }

        countElement.textContent = countText;
        submitButton.disabled = false;
        submitButton.classList.remove('btn-secondary');
        submitButton.classList.add('btn-success');
    } else {
        countElement.textContent = maxQuestions > 0 ?
            'No questions selected (Max: ' + maxQuestions + ')' :
            'No questions selected';
        countElement.style.color = ''; // Reset color
        countElement.style.fontWeight = 'normal';
        submitButton.disabled = true;
        submitButton.classList.remove('btn-success');
        submitButton.classList.add('btn-secondary');
    }
}

// Function to be added to SearchBySharedExam.cshtml
function initializeSearchPageWithBuildExamData() {
    // Check if we have stored form data from Build Exam page
    var storedFormData = sessionStorage.getItem('buildExamFormData');

    if (storedFormData) {
        try {
            var formData = JSON.parse(storedFormData);

            // Store the form data globally for use when adding questions
            window.buildExamFormData = formData;

            // Store the question limit separately for easy access
            if (formData.NoofQuestions) {
                sessionStorage.setItem('examQuestionLimit', formData.NoofQuestions);
                //console.log('Stored question limit:', formData.NoofQuestions);
            }

            // Update the submit button text to indicate it will add to existing exam
            var submitBtn = $('#submitSelectedQuestions');
            if (submitBtn.length && formData.ExamName) {
                submitBtn.html('<i class="fa fa-plus"></i> Add Selected Questions to "' + formData.ExamName + '"');
            }

            // Clear the stored data
            sessionStorage.removeItem('buildExamFormData');

            //console.log('Build Exam form data loaded:', formData);

            // Update the count display to show the limit
            updateSelectedCount();
        } catch (error) {
            console.error('Error parsing stored form data:', error);
        }
    }

    // Also check if there's a limit already on the page
    var currentLimit = getMaxAllowedQuestions();
    if (currentLimit > 0) {
        //console.log('Question limit detected:', currentLimit);
        updateSelectedCount();
    }
}

function submitSelectedQuestionsFormHidden() {

    if (selectedQuestionIds.length === 0) {
        alert('Please select at least one question.');
        return;
    }

    // Additional validation before submission
    var maxQuestions = getMaxAllowedQuestions();
    if (maxQuestions > 0 && selectedQuestionIds.length > maxQuestions) {
        alert('Cannot submit. You have selected ' + selectedQuestionIds.length +
            ' questions but the maximum allowed is ' + maxQuestions +
            '. Please remove ' + (selectedQuestionIds.length - maxQuestions) + ' question(s).');
        return;
    }

    // Check if we have existing build exam form data
    var formData = window.buildExamFormData || {};

    // Populate the hidden form fields with existing data or defaults
    $('#hiddenSearchTerm').val(formData.SearchTerm);
    $('#hiddenSelectedQuestions').val(selectedQuestionIds.join(','));
    $('#hiddenNumberOfQuestions').val(selectedQuestionIds.length);
    $('#hiddenExamName').val(formData.ExamName || $('#ExamName').val());
    $('#hiddenExamType').val(formData.ExamType || $('#ExamType').val());
    $('#hiddenEmailId').val(formData.EmailId || $('#EmailId').val() || '');
    $('#hiddenShowDetailedAnswers').val(formData.ShowDetailedAnswers || $('#ShowDetailedAnswers').is(':checked'));
    $('#hiddenShowAnsAfterExamCompletion').val(formData.ShowAnsAfterExamCompletion || $('#ShowAnsAfterExamCompletion').is(':checked'));
    $('#hiddenDateRangeEnabled').val(formData.DateRangeEnabled || $('#DateRangeEnabled').is(':checked'));
    $('#hiddenStartDate').val(formData.StartDate || $('#StartDate').val() || '');
    $('#hiddenEndDate').val(formData.EndDate || $('#EndDate').val() || '');
    $('#hiddenExamMode').val(formData.ExamMode || $('#ExamMode').val() || '');
    $('#hiddenExamAnswerToShow').val(formData.ExamAnswerToShow || $('#ExamAnswerToShow').is(':checked'));
    $('#hiddenExamTimeType').val(formData.ExamTimeType || $('#ExamTimeType').val() || '');
    $('#hiddenNumberOfCustomisedQuestions').val(formData.NumberOfCustomisedQuestions || $('#NumberOfCustomisedQuestions').val() || '');
    $('#hiddenNoofQuestions').val(formData.NoofQuestions || '');
    $('#hiddenTypeofCategoryList').val(formData.TypeofCategoryList ? formData.TypeofCategoryList.join(',') : '');
    $('#hiddenTypeofQuestionList').val(formData.TypeofQuestionList ? formData.TypeofQuestionList.join(',') : '');

    // Submit the hidden form
    $('#hiddenSubmitForm').submit();
}

// Initialize when document is ready
$(document).ready(function () {
    // Initialize the page with build exam data if present
    initializeSearchPageWithBuildExamData();

    updateSelectedCount();

    // Handle add to question list button clicks
    $(document).off('click', '.addToQuestionListBtn').on('click', '.addToQuestionListBtn', function (e) {
        e.preventDefault();
        e.stopPropagation();

        var questionId = $(this).data('question-id');
        var added = addToQuestionList(questionId);

        if (added) {
            // Toggle button visibility only if question was successfully added
            $(this).addClass('hidden');
            $('#removeFromQuestionList' + questionId).removeClass('hidden');
        }
        // If not added (due to limit), buttons remain unchanged
    });

    // Handle remove from question list button clicks
    $(document).off('click', '.removeFromQuestionListBtn').on('click', '.removeFromQuestionListBtn', function (e) {
        e.preventDefault();
        e.stopPropagation();

        var questionId = $(this).data('question-id');
        removeFromQuestionList(questionId);

        // Toggle button visibility
        $(this).addClass('hidden');
        $('#addToQuestionList' + questionId).removeClass('hidden');
    });
});