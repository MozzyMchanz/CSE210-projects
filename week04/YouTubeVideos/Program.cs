using System;
using System.Collections.Generic;

public class Comment
{
    private string commenterName;
    private string commentText;

    public Comment(string commenterName, string commentText)
    {
        this.commenterName = commenterName;
        this.commentText = commentText;
    }

    public string CommenterName => commenterName;
    public string CommentText => commentText;
}

public class Video
{
    private string title;
    private string author;
    private int lengthSeconds;
    private List<Comment> comments;

    public Video(string title, string author, int lengthSeconds)
    {
        this.title = title;
        this.author = author;
        this.lengthSeconds = lengthSeconds;
        comments = new List<Comment>();
    }

    public string Title => title;
    public string Author => author;
    public int LengthSeconds => lengthSeconds;
    public List<Comment> Comments => comments;

    public void AddComment(Comment comment)
    {
        comments.Add(comment);
    }

    public int GetNumberOfComments()
    {
        return comments.Count;
    }
}

class Program
{
    static void Main(string[] args)
    {
        List<Video> videos = new List<Video>();

        // Video 1
        Video video1 = new Video("C# Abstraction Principles", "BYU CSE210", 720);
        video1.AddComment(new Comment("Alice", "Great explanation of abstraction!"));
        video1.AddComment(new Comment("Bob", "Love the YouTube example."));
        video1.AddComment(new Comment("Charlie", "Clear and concise."));
        video1.AddComment(new Comment("Diana", "Helped me understand classes better."));
        videos.Add(video1);

        // Video 2
        Video video2 = new Video("OOP with Classes", "Jane Smith", 900);
        video2.AddComment(new Comment("Eve", "Excellent OOP tutorial."));
        video2.AddComment(new Comment("Frank", "The examples were perfect."));
        video2.AddComment(new Comment("Grace", "Thanks for this!"));
        videos.Add(video2);

        // Video 3
        Video video3 = new Video("Building Video Classes", "Alice Johnson", 600);
        video3.AddComment(new Comment("Henry", "Simple and effective."));
        video3.AddComment(new Comment("Ivy", "Good use of lists."));
        video3.AddComment(new Comment("Jack", "Perfect for beginners."));
        videos.Add(video3);

        // Video 4
        Video video4 = new Video("YouTube Analytics Demo", "Bob Wilson", 1200);
        video4.AddComment(new Comment("Kathy", "Insightful analysis."));
        video4.AddComment(new Comment("Leo", "Long but worth it."));
        video4.AddComment(new Comment("Mia", "Awesome content."));
        video4.AddComment(new Comment("Noah", "Bookmarked for later."));
        videos.Add(video4);

        // Display all videos
        foreach (Video video in videos)
        {
            Console.WriteLine($"Title: {video.Title}");
            Console.WriteLine($"Author: {video.Author}");
            Console.WriteLine($"Length: {video.LengthSeconds} seconds");
            Console.WriteLine($"Number of comments: {video.GetNumberOfComments()}");

            foreach (Comment comment in video.Comments)
            {
                Console.WriteLine($"  {comment.CommenterName}: {comment.CommentText}");
            }

            Console.WriteLine(); // Empty line between videos
        }
    }
}

