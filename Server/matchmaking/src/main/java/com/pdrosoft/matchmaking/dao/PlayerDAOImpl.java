package com.pdrosoft.matchmaking.dao;

import java.util.List;
import java.util.Optional;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.pdrosoft.matchmaking.model.Game;
import com.pdrosoft.matchmaking.model.GamePlayer;
import com.pdrosoft.matchmaking.model.GamePlayerId;
import com.pdrosoft.matchmaking.model.Player;
import com.pdrosoft.matchmaking.repository.GameRepository;

import jakarta.persistence.EntityManager;
import jakarta.persistence.PersistenceContext;

@Service
public class PlayerDAOImpl {
	@PersistenceContext
	private EntityManager em;

	@Autowired
	private GameRepository gameRepository;

	public Optional<Player> findPlayersByName(String userName) {
		var cb = em.getCriteriaBuilder();
		var cq = cb.createQuery(Player.class);
		var root = cq.from(Player.class);
		cq.select(root).where(cb.equal(root.get("userName"), userName));

		return em.createQuery(cq).getResultStream().findAny();
	}

	public void createGameWithCreator(Player creator, String gameName) {
		var game = new Game();
		game.setName(gameName);

		var link = new GamePlayer();
		link.setGame(game);
		link.setPlayer(creator);
		link.setRole("creator");
		link.setId(new GamePlayerId());

		game.setPlayerLinks(List.of(link));
		creator.getGameLinks().add(link);

		gameRepository.save(game); // Cascade saves everything
	}
}
