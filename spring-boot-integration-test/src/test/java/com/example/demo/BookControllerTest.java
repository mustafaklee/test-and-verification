package com.example.demo;

import com.example.demo.controller.*;
import com.example.demo.model.*;
import com.example.demo.service.*;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.junit.jupiter.api.*;
import org.springframework.beans.factory.annotation.*;
import org.springframework.http.MediaType;
import org.springframework.test.context.bean.override.mockito.*;
import org.springframework.test.web.servlet.*;
import org.springframework.test.web.servlet.request.*;
import java.util.*;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.*;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.*;
import static org.mockito.Mockito.*;

import org.springframework.boot.test.autoconfigure.web.servlet.WebMvcTest;

//Test ettiğimiz sınıfı yazıyoruz.
@WebMvcTest(BookController.class)
public class BookControllerTest {

    @Autowired
    private MockMvc mockMvc;

    @MockitoBean
    private BookService bookService;

    @Autowired
    private ObjectMapper objectMapper;


    @Test
    public void testGetAllBooks() throws Exception {

        //senaryoyu hazırlama işlemi...
        when(bookService.getAllBooks()).thenReturn(Arrays.asList(
                new Book(1L,"Kitap-1","Yazar-1"),
                new Book(2L,"Kitap-2","Yazar-2")
        ));

        mockMvc.perform(get("/api/books"))
                .andExpect(status().isOk())
                .andExpect(jsonPath("$.size()").value(2));
    }

    @Test
    public void testAddBook() throws Exception{
        Book b1 = new Book(1L,"Kitap-1","Yazar-1");

        when(bookService.addBook(any(Book.class))).thenReturn(b1);

        mockMvc.perform(post("/api/books")
                .contentType(MediaType.APPLICATION_JSON)
                .content(objectMapper.writeValueAsString(b1)))
                //şimdi beklediklerimizi alma zamanı..
                .andExpect(status().is(200))
                .andExpect(jsonPath("$.id").value(b1.getId()))
                .andExpect(jsonPath("$.title").value(b1.getTitle()));

    }


    @Test
    public void testGetBookById() throws Exception {
        Book b1 = new Book(1L,"Kitap-1","Yazar-1");
        long id = 1L;

        when(bookService.getBookById(id)).thenReturn(b1);


        mockMvc.perform(get("/api/books/"+id))
                .andExpect(status().isOk())
                .andExpect(jsonPath("$.id").value(id))
                .andExpect(jsonPath("$.title").value(b1.getTitle()));

    }


    @Test
    public void testDeleteBook() throws Exception{

        when(bookService.deleteBookById(anyLong())).thenReturn(true);

        mockMvc.perform(delete("/api/books/"+anyLong()))
                .andExpect(status().is(200))
                .andExpect(content().string("Book deleted Successfully!"));

    }


    @Test
    public void testDeleteBook_NotFound() throws Exception{
        Book b1 = new Book(1L,"Kitap-1","Yazar-1");

        when(bookService.deleteBookById(anyLong())).thenReturn(false);

        mockMvc.perform(delete("/api/books/"+b1.getId()))
                .andExpect(status().isOk())
                .andExpect(content().string("Book Not Found"));

    }




}
