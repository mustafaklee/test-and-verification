package com.example.demo;

import com.example.demo.model.Book;
import com.example.demo.service.BookService;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

import java.util.List;

import static org.junit.jupiter.api.Assertions.*;

public class BookServiceTest {
    private BookService bookService;


    @BeforeEach
    public void setUp()
    {
        bookService = new BookService();
    }

    @Test
    public void testGetAllBooks_EmptyResult(){
        List<Book> books = bookService.getAllBooks();
        assertNull(books);
    }

    @Test
    public void testGetAllBooks(){
        Book b1= new Book(1L,"Kitap-1","Yazar 1");
        Book b2= new Book(2L,"Kitap-2","Yazar 2");
        Book b3= new Book(3L,"Kitap-3","Yazar 3");
        Book b4= new Book(4L,"Kitap-4","Yazar 4");

        bookService.addBook(b1);
        bookService.addBook(b2);
        bookService.addBook(b3);
        bookService.addBook(b4);

        List<Book> bookList = bookService.getAllBooks();

        assertEquals(4,bookList.size());
        assertNotNull(bookList);
    }

    @Test
    public void testAddBook(){
        Book book = new Book(1L,"Kitap-1","Yazar 1");
        Book addedBook = bookService.addBook(book);

        assertEquals(addedBook.getId(),book.getId());
        assertEquals(addedBook.getTitle(),book.getTitle());
        assertNotNull(addedBook);

    }

    @Test
    public void testNullAddBook(){
        Book book = new Book(-1L,"Kitap-1","Yazar 1");

        Book addedBook = bookService.addBook(book);
        assertNull(addedBook);
    }

    @Test
    public void testGetBookById(){
        Book book = new Book(1L,"Kitap-1","Yazar 1");
        Book addedBook = bookService.addBook(book);

        Book getBook = bookService.getBookById(1L);

        assertNotNull(getBook);
        assertEquals(getBook.getId(),addedBook.getId());
        assertEquals(getBook.getTitle(),addedBook.getTitle());
    }

    @Test
    public void testDeleteBookById(){
        Book book = new Book(1L,"Kitap-1","Yazar 1");
        Book book2=  new Book(2L,"Kitap-2","Yazar 2");

        bookService.addBook(book);
        bookService.addBook(book2);

        boolean result = bookService.deleteBookById(2L);
        assertTrue(result);
    }

}
