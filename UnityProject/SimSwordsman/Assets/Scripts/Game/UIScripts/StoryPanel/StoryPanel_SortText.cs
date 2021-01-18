using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;


namespace GameWish.Game
{
	public class StoryPanel_SortText : Text
	{
	    private readonly string m_MarkList = "(\\！|\\!|\\？|\\?|\\，|\\,|\\。|\\.|\\《|\\》|\\）|\\)|\\：|\\:|\\“|\\‘|\\、|\\；|\\;|\\+|\\-)";
	    private StringBuilder m_StringBuilder;
	
	    public override void SetVerticesDirty()
	    {
	        var settings = GetGenerationSettings(rectTransform.rect.size);
	        cachedTextGenerator.Populate(this.text, settings);
	
	        m_StringBuilder = new StringBuilder(this.text);
	
	        IList<UILineInfo> lineList = this.cachedTextGenerator.lines;
	
	        Sort(lineList);
	
	        base.SetVerticesDirty();
	    }
	
	    private void Sort(IList<UILineInfo> lineList)
	    {
	        for (int i = 1; i < lineList.Count; i++)
	        {
	            int index = lineList[i].startCharIdx;
	            char firstLineChar = text[index];
	            //Debug.LogError(firstLineChar);
	            bool isMark = Regex.IsMatch(firstLineChar.ToString(), m_MarkList);
	   
	            if (isMark)
	            {
	                //Debug.LogError(isMark);
	                int changeIndex = index - 1;
	                m_StringBuilder.Insert(changeIndex, '\n');
	                this.text = m_StringBuilder.ToString();
	
	                Sort(lineList);
	
	                break;
	            }
	        }
	    }
	
	    protected override void OnDestroy()
	    {
	        base.OnDestroy();
	
	        m_StringBuilder.Clear();
	    }
	}
	
}