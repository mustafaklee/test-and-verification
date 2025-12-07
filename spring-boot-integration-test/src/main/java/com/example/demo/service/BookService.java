package com.example.demo.service;


import com.example.demo.model.Book;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;

@Service
public class BookService {
    private final List<Book> books = new ArrayList<>();

    public List<Book> getAllBooks(){
        if(books.isEmpty())
            return null;
        else return books;
    }

    public Book getBookById(Long id){
        return books
                .stream()
                .filter(book->book.getId().equals(id))
                .findFirst()
                .orElse(null);
    }

    public Book addBook(Book book){
        if(book.getId() < 0)
            return null;
        else  books.add(book);
        return book;
    }

    public boolean deleteBookById(Long id){
        return books.removeIf(p->p.getId().equals(id));
    }
}

