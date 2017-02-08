using System.ComponentModel.DataAnnotations;

namespace FontColorExperiment.Models.Question
{
    public enum QuestionType
    {
        [Display(Name = "Radio Button")]
        Radio,
        [Display(Name = "Checkbox")]
        Checkbox,
        [Display(Name = "Drop-down list")]
        Dropdown,
        [Display(Name = "Textbox")]
        TextBox,
        [Display(Name = "Multiple Choice Grid")]
        MultiChoiceGrid
    }
}
