package com.pdrosoft.matchmaking.controller;

import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import lombok.Builder;
import lombok.Value;

@RestController
@RequestMapping("/api")
public class GameApiController {

	@Value
	@Builder
	private static class Result {
		public String message;
	}
	
	@GetMapping(path = "/test", produces = { "application/json" })
	public Result testController() {
		return Result.builder().message("hello world").build();
	}
}
