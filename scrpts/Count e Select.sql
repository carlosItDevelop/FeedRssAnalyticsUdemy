Select AuthorId, COUNT(*) 
	   as Total from ArticleMatrices 
	   where AuthorId = 'sourabh-sharma43' group by AuthorId;

SELECT [Id],[ViewsCount],[Views],[Likes],[Title],[Type],[Category],[PubDate]
  FROM ArticleMatrices where AuthorId = 'sourabh-sharma43' order by [Title];
  