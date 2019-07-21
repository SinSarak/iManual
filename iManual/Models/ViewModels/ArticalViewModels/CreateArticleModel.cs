using iManual.Models.ViewModels.ArticleContentViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iManual.Models.ViewModels.ArticalViewModels
{
    public class CreateArticleModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int SubCategoryId { get; set; }
        public string JsonContents { get; set; }


        public List<CreateArticleContentwithArticleModel> GetArticleContents()
        {
            return JsonConvert.DeserializeObject<List<CreateArticleContentwithArticleModel>>(this.JsonContents);
        }
    }
}